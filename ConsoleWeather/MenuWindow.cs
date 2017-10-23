using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using ConsoleDraw;
using ConsoleDraw.Inputs;
using ConsoleDraw.Windows.Base;

namespace ConsoleWeather
{
    public class MenuWindow : FullWindow
    {
        private TextArea errorTextArea;
        private TextArea primaryCityArea;
        private TextArea secondaryCityArea;
        private TextArea favCityArea;

        public MenuWindow() : base(0, 0, Console.WindowWidth, Console.WindowHeight, null)
        {
            Label bannerLabel = new Label(
                "    _____                      _   __          __        _   _               \r\n    / ____|                    | |  \\ \\        / /       | | | |              \r\n   | |     ___  _ __  ___  ___ | | __\\ \\  /\\  / /__  __ _| |_| |__   ___ _ __ \r\n   | |    / _ \\| \'_ \\/ __|/ _ \\| |/ _ \\ \\/  \\/ / _ \\/ _` | __| \'_ \\ / _ \\ \'__|\r\n   | |___| (_) | | | \\__ \\ (_) | |  __/\\  /\\  /  __/ (_| | |_| | | |  __/ |   \r\n    \\_____\\___/|_| |_|___/\\___/|_|\\___| \\/  \\/ \\___|\\__,_|\\__|_| |_|\\___|_|   \r\n                                                                              \r\n  "
                , 1, 1, "bannerLabel", this);

            Button customBtn = new Button(8, 34, "Custom query", "customBtn", this) { Action = delegate () { OpenCustomWindow(); } };

            primaryCityArea = new TextArea(10, 41, 40, 6, "primaryCityArea", this);
            primaryCityArea.BackgroundColour = ConsoleColor.DarkGray;
            primaryCityArea.Selectable = false;
            secondaryCityArea = new TextArea(10, 0, 40, 6, "secondaryCityArea", this);
            secondaryCityArea.BackgroundColour = ConsoleColor.DarkGray;
            secondaryCityArea.Selectable = false;
            
            Button exitBtn = new Button(18, 74, "Exit", "exitBtn", this) { Action = delegate () {ExitProgram(); } };

            errorTextArea = new TextArea(21, 0, 76, 1, "errorTextArea", this);
            errorTextArea.BackgroundColour = ConsoleColor.DarkRed;
            errorTextArea.Selectable = false;

            check:
            if (CheckForInternetConnection())
            {
                DisplayResult("Warszawa", primaryCityArea);
                DisplayResult("Krakow", secondaryCityArea);
            }
            else
            {
                DisplayError("No Internet connection... Press any key to try again.");
                goto check;
            }

            if (new FileInfo("fav.dat").Exists && new FileInfo("fav.dat").Length > 0)
            {
                Label favLabel = new Label("Your favourite: ", 17, 32, "favLabel", this);

                favCityArea = new TextArea(19, 20, 40, 6, "favArea", this);
                favCityArea.BackgroundColour = ConsoleColor.DarkGray;
                favCityArea.Selectable = false;

                using (StreamReader reader = new StreamReader("fav.dat"))
                {
                    DisplayResult(reader.ReadLine(), favCityArea);
                }

                Inputs.Add(favLabel);
                Inputs.Add(favCityArea);

                exitBtn = new Button(25, 74, "Exit", "exitBtn", this) { Action = delegate () { ExitProgram(); } };

                errorTextArea = new TextArea(26, 0, 76, 1, "errorTextArea", this);
                errorTextArea.BackgroundColour = ConsoleColor.DarkRed;
                errorTextArea.Selectable = false;
            }

            Inputs.Add(bannerLabel);
            Inputs.Add(primaryCityArea);
            Inputs.Add(secondaryCityArea);
            Inputs.Add(customBtn);
            Inputs.Add(exitBtn);

            CurrentlySelected = customBtn;
            Draw();
            MainLoop();
        }

        private void OpenCustomWindow()
        {
            WindowManager.UpdateWindow(78, 24);
            WindowManager.UpdateWindow(78, 24);
            new MainWindow();
        }

        private void DisplayResult(string cityName, TextArea box)
        {
            if (cityName == "")
            {
                DisplayError("Empty string entered... Press any key to try again.");
                return;
            }
            if (!Regex.IsMatch(cityName, @"^[a-zA-Z\x20]+$"))
            {
                DisplayError("Forbiden characters entered... Press any key to try again.");
                return;
            }
            try
            {
                var response = new APICall(cityName).GetResponse();
                var weatherData = new CityWeather();
                weatherData.ConvertResponse(response);
                box.SetText(weatherData.FormatInformation());
                Draw();
            }
            catch (Exception e)
            {
                DisplayError("Failed to get data from server... Press any key to try again.");
            }
        }

        public void DisplayError(string errorMessage)
        {
            errorTextArea.SetText(errorMessage);
            Inputs.Add(errorTextArea);
            errorTextArea.Selectable = false;
            errorTextArea.Draw();
            Console.ReadKey();
            Inputs.Remove(errorTextArea);
            Draw();
        }

        private void ExitProgram()
        {
            Environment.Exit(0);
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (client.OpenRead("http://clients3.google.com/generate_204"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}