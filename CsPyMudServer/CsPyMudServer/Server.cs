using System;

namespace CsPyMudServer
{
    class Server
    {
        private ConnectionListener connectionListener;
        private WorldManager worldManager;
        private ConnectionManager connectionManager;

        private static bool ShouldQuit;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsPyMudServer.Server"/> class.
        /// </summary>
        public Server() { }

        /// <summary>
        /// Set up the network listeners, etc. to get the server going
        /// </summary>
        public void Startup()
        {
            ShouldQuit = false;
            Console.CancelKeyPress += delegate { ShouldQuit = true; };

            // startup raw connection listener in its own thread 
            // (it will pass off successful SSL connections to ConnectionManager)
            connectionListener = new ConnectionListener(8000);

            worldManager = new WorldManager();

            connectionManager = new ConnectionManager(connectionListener, worldManager);

            worldManager.Startup();
            connectionManager.Startup();
            connectionListener.Startup();        
        }

        /// <summary>
        /// Stop all threads, release all resources, etc.
        /// </summary>
        public void Shutdown()
        {
            // abort raw connection listener thread
            connectionListener.Shutdown();

            // tell ConnectionManager to close all remaining connections
            connectionManager.Shutdown();

            // finally, shut down the world manager
            worldManager.Shutdown();
        }

        /// <summary>
        /// Called by the main loop so the server can do things without waiting
        /// for a network event
        /// </summary>
        public void Tick()
        {
            connectionManager.Tick();
            worldManager.Tick();
        }

        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            Server app = new Server();

            app.Startup();

            while(ShouldQuit == false)
            {
                app.Tick();
            }

            app.Shutdown();
        }
    }
}
