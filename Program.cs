using static SDL2.SDL;

namespace Sandbox
{
    class Program
    {
        static void Main()
        {
            SDL_Init(SDL_INIT_VIDEO); 

            IntPtr window = SDL_CreateWindow(
                "Sandbox", //window name
                SDL_WINDOWPOS_CENTERED, //initial window pos x
                SDL_WINDOWPOS_CENTERED, //initial window pos y
                1280, //window width
                720, //window height
                SDL_WindowFlags.SDL_WINDOW_SHOWN //window flags
                );

            IntPtr renderer = SDL_CreateRenderer(
                window, //specify on which window should the renderer be created
                -1,
                SDL_RendererFlags.SDL_RENDERER_ACCELERATED | //renderer flags
                SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC //vsync
                );

            Application app = new Application(window, renderer);
            app.Init();
        }
    }
}