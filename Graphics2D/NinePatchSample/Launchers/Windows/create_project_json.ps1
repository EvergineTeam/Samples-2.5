# Script to generate project.json for all packages.config file in the solution. 
# Run from root solution folder
# execute this script with powershell:
# powershell -File <filepath> 
#   [-f <targetframework] net4
Param
(
    [alias("f")]
	[string] $TargetFramework = "net45",
    [alias("r")]
    [switch]
    $redo

)

$ScriptPath = $MyInvocation.MyCommand.Path
$ScriptDir  = Split-Path -Parent $ScriptPath
$PackagesConfigFileName = 'packages.config'
if($redo){
    $PackagesConfigFileName = 'packages_old.config'
}

#Create project.json from packages.config
Get-ChildItem -path '.' -Recurse -Include $PackagesConfigFileName |
	ForEach {
		$PackageFilePath = $_.FullName
		$ProjectFilePath = $_.Directory.FullName + '\project.json'
		Write-Output $PackageFilePath
		Write-Output $ProjectFilePath
		'{
  "dependencies": {' | Out-File  $ProjectFilePath

	  (Select-xml -xpath '//package' -Path $PackageFilePath | % { """{0}"": ""{1}""" -f $_.Node.id,$_.Node.version }) -join ",`n" | Out-File $ProjectFilePath -Append
	'  },
  "frameworks": {
    "' + $TargetFramework + '": {}
  },
    "runtimes":  {
        "win-anycpu": {},
        "win": {}
    }
}' | Out-File $ProjectFilePath -Append
		Rename-Item -Path $PackageFilePath -NewName 'packages_old.config'
}

	

$MsbNS = @{msb = 'http://schemas.microsoft.com/developer/msbuild/2003'}
Get-ChildItem -path '.' -Recurse -Include '*.csproj' | ForEach {
	$CsProjFilePath = $_.FullName
	$ProjectFilePath = $_.Directory.FullName + '\project.json'
	$proj = [xml] (Get-Content $CsProjFilePath)
	
	#Remove all references to ..packages files
	$xpath = "//msb:Reference/msb:HintPath[contains(.,'..\packages')]"
	$nodes = @(Select-Xml -xpath $xpath $proj -Namespace $MsbNS | foreach {$_.Node})
	if (!$nodes) { Write-Verbose "RemoveElement: XPath $XPath not found" }
	Write-Output 'Reference Nodes found: ' $nodes.Count
	foreach($node in $nodes) {
		$referenceNode = $node.ParentNode
		$itemGroupNode = $referenceNode.ParentNode
		[void]$itemGroupNode.RemoveChild($referenceNode)
	}
	[System.XML.XMLElement] $itemGroupNoneNode = $null
	#Find itemgroup with None Elements, if not found add.
	$itemGroupNoneNodes = @(Select-Xml -xpath "//msb:ItemGroup/msb:None" $proj -Namespace $MsbNS | foreach {$_.Node})
	Write-Output '$itemGroupNoneNode found: ' $itemGroupNoneNodes.Count
	if($itemGroupNoneNodes.Count -eq 0){
		# create itemgroup element for None nodes.
		Write-Output 'Adding ItemGroup for None Nodes'
		$itemGroupNoneNode =  $proj.CreateElement('ItemGroup',$proj.DocumentElement.NamespaceURI)
		$itemGroupNodes = @(Select-Xml -xpath "//msb:ItemGroup" $proj -Namespace $MsbNS | foreach {$_.Node})
		$itemGroupNodes.Count
		[void]$proj.DocumentElement.InsertAfter($itemGroupNoneNode,$itemGroupNodes[$itemGroupNodes.Count-1])
		
	}else{
		$itemGroupNoneNode = $itemGroupNoneNodes[0].ParentNode
	}

	#Remove packages.config from ItemGroup
	$nodes = @(Select-Xml -xpath "//msb:ItemGroup/msb:None[@Include='packages.config']" $proj -Namespace $MsbNS | foreach {$_.Node})
	Write-Output 'packages.config Nodes found: ' $nodes.Count
	foreach($node in $nodes) {
		$itemGroupNode = $node.ParentNode
		[void]$itemGroupNode.RemoveChild($node)
	}
	
	#Remove build target EnsureNuGetPackageBuildImports from csproj
	$nodes = @(Select-Xml -xpath "//msb:Target[@Name='EnsureNuGetPackageBuildImports']" $proj -Namespace $MsbNS | foreach {$_.Node})
	Write-Output 'EnsureNuGetPackageBuildImports target found: ' $nodes.CountAd
	foreach($node in $nodes) {
		$itemGroupNode = $node.ParentNode
		[void]$itemGroupNode.RemoveChild($node)
	}
	
	#Add project.json to itemGroup
	if( Test-Path $ProjectFilePath){
		$nodes = @(Select-Xml -xpath "//msb:ItemGroup/msb:None[@Include='project.json']" $proj -Namespace $MsbNS | foreach {$_.Node})
		if($nodes.Count -eq 0){
			$projectJsonNoneNode = $proj.CreateElement("None", $proj.DocumentElement.NamespaceURI)
			$projectJsonNoneNode.SetAttribute("Include","project.json")
			[void]$itemGroupNoneNode.AppendChild($projectJsonNoneNode)
			Write-Output 'Adding None node for project.json'
		}
	}

    #add PropertyGroup nodes targetFrameworkProfile, CopyNuGetImplementations, PlatformTarget
    # Find the TargetFrameworkVersion to be used to find the parent PropertyGroup node
    $xpath = "//msb:PropertyGroup/msb:TargetFrameworkVersion"
    $nodes = @(Select-Xml -xpath $xpath $proj -Namespace $MsbNS | foreach {$_.Node})
    if ($nodes.Count -gt 0) {
        [System.XML.XMLElement] $node = $nodes[0]
        $propertyGroupNode = $node.ParentNode
        $nodes = @(Select-Xml -xpath "//msb:PropertyGroup/msb:TargetFrameworkProfile" $proj -Namespace $MsbNS | foreach {$_.Node})
		if($nodes.Count -eq 0){
			$node = $proj.CreateElement("TargetFrameworkProfile", $proj.DocumentElement.NamespaceURI)
			[void]$propertyGroupNode.AppendChild($node)
			Write-Output 'Adding TargetFrameworkProfile node for PropertyGroup'
		}
        #$nodes = @(Select-Xml -xpath "//msb:PropertyGroup/msb:CopyNuGetImplementations" $proj -Namespace $MsbNS | foreach {$_.Node})
		#if($nodes.Count -eq 0){
		#	$node = $proj.CreateElement("CopyNuGetImplementations", $proj.DocumentElement.NamespaceURI)
        #    $textnode = $proj.CreateTextNode("true")
        #    $node.AppendChild($textnode)
		#	[void]$propertyGroupNode.AppendChild($node)
		#	Write-Output 'Adding CopyNuGetImplementations node for PropertyGroup'
		#}
        $nodes = @(Select-Xml -xpath "//msb:PropertyGroup/msb:PlatformTarget[not(@*)]" $proj -Namespace $MsbNS | foreach {$_.Node})
		if($nodes.Count -eq 0){
			$node = $proj.CreateElement("PlatformTarget", $proj.DocumentElement.NamespaceURI)
            $textnode = $proj.CreateTextNode("AnyCPU")
            $node.AppendChild($textnode)
			[void]$propertyGroupNode.AppendChild($node)
			Write-Output 'Adding PlatformTarget node for PropertyGroup'
		}
    }
	
    # replace ToolsVersion with 14.0
    $attibutes = Select-Xml -xpath "//@ToolsVersion" $proj -Namespace $MsbNS
    foreach ($attribute in $attibutes){
            
        $attribute.Node.value = "14.0"
        Write-Output 'Setting ToolsVersion to 14.0'
    }

	$proj.Save($CsProjFilePath)
 }
