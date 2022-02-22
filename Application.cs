using static SDL2.SDL;

namespace Sandbox
{
    internal class Application
    {
        private enum BlockType { Air = 0, Water, Sand, Stone, Fire };

        private BlockType[,] map = new BlockType[20, 20];

        private bool _shouldExit = false;
        public IntPtr Window { get; private set; }
        public IntPtr Renderer { get; private set; }

        public Application(IntPtr window, IntPtr renderer)
        {
            Window = window;
            Renderer = renderer;
        }

        private void Render()
        {
            SDL_SetRenderDrawColor(Renderer, 33, 33, 33, 255); //set the clear color 
            SDL_RenderClear(Renderer); //clear the entire window with the set color

            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 20; x++)
                {
                    SetRenderDrawColor(x, y);

                    SDL_Rect rect = new SDL_Rect
                    {
                        x = (32 * x),
                        y = (32 * y),
                        w = 32,
                        h = 32
                    };

                    SDL_RenderFillRect(Renderer, ref rect);
                }
            }

            SDL_RenderPresent(Renderer); //swap
        }

        private void SetRenderDrawColor(int x, int y)
        {
            switch(map[x, y])
            {
                case BlockType.Air:
                    SDL_SetRenderDrawColor(Renderer, 44, 44, 44, 255);
                    break;
                case BlockType.Water:
                    SDL_SetRenderDrawColor(Renderer, 33, 33, 255, 255);
                    break;
                case BlockType.Sand:
                    SDL_SetRenderDrawColor(Renderer, 255, 255, 0, 255);
                    break;
                case BlockType.Stone:
                    SDL_SetRenderDrawColor(Renderer, 60, 60, 60, 255);
                    break;
                case BlockType.Fire:
                    SDL_SetRenderDrawColor(Renderer, 222, 33, 33, 255);
                    break;
            }
        }

        public void Init()
        {
            map[5, 4] = BlockType.Air;
            map[5, 5] = BlockType.Water;
            map[5, 6] = BlockType.Sand;
            map[5, 7] = BlockType.Stone;
            map[5, 8] = BlockType.Fire;

            while (!_shouldExit) //main loop
            {
                PollEvents();
                Render();
            }

            Cleanup();
        }

        private void PollEvents()
        {
            while (SDL_PollEvent(out SDL_Event e) == 1)
            {
                switch (e.type)
                {
                    case SDL_EventType.SDL_QUIT:
                        _shouldExit = true;
                        break;
                }
            }
        }

        private void Cleanup()
        {
            SDL_DestroyRenderer(Renderer);
            SDL_DestroyWindow(Window);
            SDL_Quit();
        }
    }
}
