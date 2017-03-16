#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
using WaveEngine.Vuforia;
#endregion

namespace VuforiaTest
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

            ScreenContext screenContext = new ScreenContext(new MyScene());
            WaveServices.ScreenContextManager.To(screenContext);

            string liscenseKey = "AeRNu+n/////AAAAGfmXotSIvkSQp6D3annJaEEIdl9kN9/8PT8UYO0GOlbvz/n/w4oqvYxTLeLJ/mcx6ulZ+nPitOux8SEyXlTbeYvrM7qO63OkYLluwhT/ULICxNp+z9UIKaLiJDPSKdBdlH3h0I2xzzmo46D7urlvhOxt0l+sNDchaqrQLbV2o7lkvnhGJCzAQl/ZDjsYrEWU+98ZT45DSMNBnHOuvnG1IDoQpsdW7e58yH7bwxqLSbCoBJqWD0iFL9IBXrkbOtrqyrA0UCDqeKl8JeIb4+UKQKJR+WV6Hdml3FafdiDAUoPBwK92363ndni2ai+fEtkTmJFRUxJunoaqEpzC6kr1QYvL/d7JwaBCGfZRKfcp7QD1";
            WaveServices.RegisterService(new VuforiaService(WaveContent.Assets.AR.TestVuforia_xml, liscenseKey));
        }
    }
}
