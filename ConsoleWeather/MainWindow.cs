using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using ConsoleDraw;
using ConsoleDraw.Inputs;
using ConsoleDraw.Windows.Base;

namespace ConsoleWeather
{

    public class MainWindow : FullWindow
    {
        private TextBox Display;
        private TextArea resultTextArea;
        private TextArea errorTextArea;
        private Button favBtn;

        public MainWindow() : base(0, 0, Console.WindowWidth, Console.WindowHeight, null)
        {
            Label bannerLabel = new Label(
                "  _____                      _   __          __        _   _               \r\n  / ____|                    | |  \\ \\        / /       | | | |              \r\n | |     ___  _ __  ___  ___ | | __\\ \\  /\\  / /__  __ _| |_| |__   ___ _ __ \r\n | |    / _ \\| \'_ \\/ __|/ _ \\| |/ _ \\ \\/  \\/ / _ \\/ _` | __| \'_ \\ / _ \\ \'__|\r\n | |___| (_) | | | \\__ \\ (_) | |  __/\\  /\\  /  __/ (_| | |_| | | |  __/ |   \r\n  \\_____\\___/|_| |_|___/\\___/|_|\\___| \\/  \\/ \\___|\\__,_|\\__|_| |_|\\___|_|   \r\n                                                                            \r\n  "
                , 1, 1, "bannerLabel", this);


            Label lbl = new Label("Type in city name: ", 8, 30, "lbl", this);

            Display = new TextBox(9, 30, "", "displayTxtBox", this, 21) { Selectable = false };

            Button confirmBtn = new Button(11, 35, " Confirm ", "confirmBtn", this) { Action = delegate () { DisplayResult(Display.GetText()); } };

            resultTextArea = new TextArea(14, 20, 40, 6, "resultTextArea", this);
            resultTextArea.BackgroundColour = ConsoleColor.DarkGray;
            resultTextArea.Selectable = false;

            favBtn = new Button(21, 46, "Add Favourite", "favBtn", this) { Action = delegate () { AddFavourite(); } };

            Button closeBtn = new Button(21, 63, "Close", "closeBtn", this) { Action = delegate () { CloseWindow(); } };

            errorTextArea = new TextArea(22, 0, 76, 1, "errorTextArea", this);
            errorTextArea.BackgroundColour = ConsoleColor.DarkRed;
            errorTextArea.Selectable = false;

            Display.Selectable = true;

            Inputs.Add(bannerLabel);

            Inputs.Add(lbl);
            Inputs.Add(Display);

            Inputs.Add(confirmBtn);

            Inputs.Add(closeBtn);

            Inputs.Add(resultTextArea);

            CurrentlySelected = Display;
            Draw();
            MainLoop();
        }

        private void CloseWindow()
        {
            if (new FileInfo("fav.dat").Exists && new FileInfo("fav.dat").Length > 0)
            {
                WindowManager.UpdateWindow(84, 28);
                WindowManager.UpdateWindow(84, 28);
            }
            else
            {
                WindowManager.UpdateWindow(84, 20);
                WindowManager.UpdateWindow(84, 20);
            }
            new MenuWindow();
        }

        private void AddFavourite()
        {
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "fav.dat", Display.GetText());
            Label confirmationLabel = new Label("Added to favourites!", 21, 20, "confirmationLabel", this);
            Inputs.Add(confirmationLabel);
            confirmationLabel.Draw();
            Console.ReadKey();
            Inputs.Remove(confirmationLabel);
            Draw();
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

        private void DisplayResult(string cityName)
        {
            if (cityName == "")
            {
                DisplayError("Empty string entered... Press any key to try again.");
                return;
            }
            if (!Regex.IsMatch(cityName, @"^[a-zA-Z]+$"))
            {
                DisplayError("Forbiden characters entered... Press any key to try again.");
                return;
            }
            try
            {
                var response = new APICall(cityName).GetResponse();
                var weatherData = new CityWeather();
                weatherData.ConvertResponse(response);
                resultTextArea.SetText(weatherData.FormatInformation());
                Inputs.Add(favBtn);
                Draw();
            }
            catch (Exception e)
            {
                DisplayError("Failed to get data from server... Press any key to try again.");
            }
        }

    }
}