using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using MathLibrary;
using System.Diagnostics;

namespace RiggableDice
{
    internal class Game
    {
        private static List<Scene> _scenes;
        private static Scene _currentScene;

        public static Scene CurrentScene 
        { 
            get => _currentScene; 

            set
            {
                if (_currentScene != null)
                    _currentScene.End();
                _currentScene = value;
                _currentScene.Start();
            }
        }

        public Game()
        {
            _scenes = new List<Scene>();
            // add starting scene
        }

        public static bool AddScene(Scene scene)
        {
            bool sceneAdded = false;

            if (!_scenes.Contains(scene))
            {
                _scenes.Add(scene);
                sceneAdded = true;
            }


            if (_currentScene == null)
                CurrentScene = scene;

            return sceneAdded;
        }

        public static bool RemoveScene(Scene scene)
        {
            bool removed = _scenes.Remove(scene);

                if (_currentScene == scene)
                    CurrentScene = GetScene(0);

            return removed;
        }

        public static Scene GetScene(int index)
        {
            if (_scenes.Count <= 0 || _scenes.Count <= index || index < 0)
                return null;

            return _scenes[index];
        }

        public void Run()
        {
            Raylib.InitWindow(800, 800, "Dice");

            // timing stuff
            Raylib.SetTargetFPS(60);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            double deltaTime = 1;
            long currentTime = 0;
            long lastTime = 0;

            Scene gameScene = new TitleScene();
            AddScene(gameScene);

            while (!Raylib.WindowShouldClose())
            {
                currentTime = stopwatch.ElapsedMilliseconds;

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                CurrentScene.Update(deltaTime);

                Raylib.EndDrawing();

                deltaTime = (currentTime - lastTime) / 1000.0;
                lastTime = currentTime;
            }

            CurrentScene.End();

            Raylib.CloseWindow();
        }
    }
}
