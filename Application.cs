using static SDL2.SDL;

namespace Sandbox
{
    internal class Application
    {
        private enum BlockType { Air = 0, Water, Sand, Stone, Fire };

        private BlockType[,] map = new BlockType[100, 100];

        private bool _shouldExit = false;
        public int mouseDevision = 10; // Scalling of the blocks, feel free to change ;)
        public IntPtr Window { get; private set; }
        public IntPtr Renderer { get; private set; }

        public int brushSize  = 4;

        public Application(IntPtr window, IntPtr renderer)
        {
            Window = window;
            Renderer = renderer;
        }

        private void Render()
        {
            SDL_SetRenderDrawColor(Renderer, 33, 33, 33, 255); //set the clear color 
            SDL_RenderClear(Renderer); //clear the entire window with the set color

            
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    SetRenderDrawColor(x, y);

                    SDL_Rect rect = new SDL_Rect
                    {
                        x = (map.GetLength(0)/ (map.GetLength(0) / mouseDevision) * x), 
                        y = (map.GetLength(0)/ (map.GetLength(0) / mouseDevision) * y), 
                        w = map.GetLength(0) / (map.GetLength(0) / mouseDevision),      
                        h = map.GetLength(0) / (map.GetLength(0) / mouseDevision)       
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
                    case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                        Place(e);
                        break;
                    case SDL_EventType.SDL_QUIT:
                        _shouldExit = true;
                        break;
                }
            }
        }
        public void Place(SDL_Event e)
        {

            if (e.motion.x / mouseDevision < map.GetLength(0) &&   // Checking if we are clicking on the map itself and not outside of it.
                e.motion.x > 0 &&                       // Checking if we are clicking on the map itself and not outside of it.
                e.motion.y / mouseDevision < map.GetLength(1) &&   // Checking if we are clicking on the map itself and not outside of it.     
                e.motion.y > 0                          // Checking if we are clicking on the map itself and not outside of it.
                )
            {
                for (int x = -brushSize + 1; x < brushSize; x++) // Adding the one makes the brush round itead of a sharp square.
                {
                    for (int y = -brushSize + 1; y < brushSize; y++) // Adding the one makes the brush round itead of a sharp square.
                    {
                        if(x + y <= brushSize && x + y >= -brushSize && x - y <= brushSize && x - y >= -brushSize) // Makes the round shape.
                        {
                            if(e.motion.x / mouseDevision + x < map.GetLength(0) && e.motion.x / mouseDevision + x > 0 &&
                               e.motion.y / mouseDevision + y < map.GetLength(1) && e.motion.y / mouseDevision + y > 0 )
                            map[(int)(e.motion.x / mouseDevision) + x, (int)(e.motion.y / mouseDevision) + y] = BlockType.Sand; // Sets the positions to sand.
                        }
                    }
                }
            }

            Render();
        }

        private void Cleanup()
        {
            SDL_DestroyRenderer(Renderer);
            SDL_DestroyWindow(Window);
            SDL_Quit();
        }
    }
}
