using System;
using System.Collections.Generic;
using System.Threading;

namespace game
{
    class Map // Карта уровня
    {
        private int sizeCameraFOV; // Диапазон размеров карты
        private char[,] _map; // Карта

        public Map(int sizeCameraFOV)
        {
            this.sizeCameraFOV = sizeCameraFOV;
            Generate();
        }

        public char[,] GetMap { get { return _map; } }

        private void Generate() // Генерация карты
        {
            _map = new char[Game._random.Next((int)(sizeCameraFOV * 0.75), (int)(sizeCameraFOV + 1.25)),
                           Game._random.Next((int)(sizeCameraFOV * 0.75), (int)(sizeCameraFOV + 1.25))];
            for (int i = 0; i < _map.GetLength(0); i++) // Внешние стены
            {
                _map[i, 0] = ':';
                _map[i, _map.GetLength(1) - 2] = ':';
            }
            for (int i = 1; i < _map.GetLength(1) - 1; i++)
            {
                _map[0, i] = ':';
                _map[_map.GetLength(0) - 2, i] = ':';
            }
            for (int i = 1; i < _map.GetLength(0) - 1; i++) // Стены
            {
                for (int j = 1; j < _map.GetLength(1) - 1; j++)
                {
                    /*if (i > 1 && j > 1 && i < _map.GetLength(0) - 2 && j < _map.GetLength(1) - 2 && _map[i, j] != ':' &&
                       (_map[i - 1, j] == ':' || _map[i - 1, j + 1] == ':' || _map[i, j + 1] == ':' || _map[i + 1, j + 1] == ':' ||
                        _map[i + 1, j] == ':' || _map[i + 1, j - 1] == ':' || _map[i, j - 1] == ':' || _map[i - 1, j - 1] == ':'))*/
                    if (i > 1 && j > 1 && i < _map.GetLength(0) - 2 && j < _map.GetLength(1) - 2 && _map[i, j] != ':' &&
                       (_map[i - 1, j] == ':'))
                    {
                        _map[i, j] = Game._random.Next(0, 100) < 50 ? ':' : '.';
                    }
                    else if (i > 1 && j > 1 && i < _map.GetLength(0) - 2 && j < _map.GetLength(1) - 2 && _map[i, j] != ':' &&
                       (_map[i, j - 1] == ':'))
                    {
                        _map[i, j] = Game._random.Next(0, 100) < 50 ? ':' : '.';
                    }
                    else if (i > 1 && j > 1 && i < _map.GetLength(0) - 2 && j < _map.GetLength(1) - 2 && _map[i, j] != ':' &&
                       (_map[i - 1, j] == ':' || _map[i, j - 1] == ':'))
                    {
                        _map[i, j] = Game._random.Next(0, 100) < 25 ? ':' : '.';
                    }
                    else if (_map[i, j] != ':')
                    {
                        _map[i, j] = Game._random.Next(0, 100) < 1 ? ':' : '.';
                    }
                }
            }
            for (int i = 1; i < _map.GetLength(0) - 1; i++) // Стены
            {
                for (int j = 1; j < _map.GetLength(1) - 1; j++)
                {
                    if (i > 1 && j > 1 && i < _map.GetLength(0) - 2 && j < _map.GetLength(1) - 2 && _map[i, j] != ':' &&
                       (_map[i - 1, j] == ':' && _map[i + 1, j] == ':' && _map[i, j - 1] == ':' && _map[i, j + 1] == ':'))
                    {
                        _map[i, j] = Game._random.Next(0, 100) < 50 ? ':' : '.';
                    }
                }
            }
        }
    }

    class Monster // Монстр
    {
        private string name;
        private char icon;
        private int x;
        private int y;
        private int health;
        private int mana;
        private int maxHealth;
        private int maxMana;

        public Monster(string name, char icon, int health, int mana)
        {
            this.name = name;
            this.icon = icon;
            this.health = health;
            this.mana = mana;
            maxHealth = health;
            maxMana = mana;
            Locate(Game._map.GetMap);
        }

        public char GetIcon { get { return icon; } }

        public int[] GetPosition { get { return new int[] { x, y }; } }

        private bool isWall(int derX, int derY)
        {
            if (Game._map.GetMap[y + derY, x + derX] == ':')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Locate(char[,] _map)
        {
            while (true)
            {
                x = Game._random.Next(1, _map.GetLength(1) - 2);
                y = Game._random.Next(1, _map.GetLength(0) - 2);
                if (_map[y, x] != ':')
                {
                    if (Game._player.GetPosition[0] == x && Game._player.GetPosition[1] == y)
                    {
                        continue;
                    }
                    foreach (var m in Game._monsters)
                    {
                        if (m.GetPosition[0] == x && m.GetPosition[1] == y)
                        {
                            continue;
                        }
                    }
                }
                return;
            }
        }

        public void DoAction() // Действия монстра
        {
            switch (Game._random.Next(1, 10))
            {
                case 1:
                    {
                        if (!isWall(0, -1))
                        {
                            y -= 1;
                        }
                        break;
                    }
                case 2:
                    {
                        if (!isWall(1, -1))
                        {
                            y -= 1;
                            x += 1;
                        }
                        break;
                    }
                case 3:
                    {
                        if (!isWall(1, 0))
                        {
                            x += 1;
                        }
                        break;
                    }
                case 4:
                    {
                        if (!isWall(1, 1))
                        {
                            y += 1;
                            x += 1;
                        }
                        break;
                    }
                case 5:
                    {
                        if (!isWall(0, 1))
                        {
                            y += 1;
                        }
                        break;
                    }
                case 6:
                    {
                        if (!isWall(-1, 1))
                        {
                            y += 1;
                            x -= 1;
                        }
                        break;
                    }
                case 7:
                    {
                        if (!isWall(-1, 0))
                        {
                            x -= 1;
                        }
                        break;
                    }
                case 8:
                    {
                        if (!isWall(-1, -1))
                        {
                            y -= 1;
                            x -= 1;
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }

    class Player // Игрок
    {
        private string name;
        private char icon;
        private int x;
        private int y;
        private int viewRadius;
        private int health;
        private int mana;
        private int maxHealth;
        private int maxMana;

        public Player(string name, char icon, int health, int mana, int viewRadius)
        {
            this.name = name;
            this.icon = icon;
            this.health = health;
            this.mana = mana;
            maxHealth = health;
            maxMana = mana;
            this.viewRadius = viewRadius;
            Locate(Game._map.GetMap);
            Thread Action = new Thread(DoAction);
            Action.Start();
        }

        public char GetIcon { get { return icon; } }

        public int[] GetPosition { get { return new int[] { x, y }; } }

        public int GetHealth { get { return health; } }

        public int GetMana { get { return mana; } }

        public int GetMaxHealth { get { return maxHealth; } }

        public int GetMaxMana { get { return maxMana; } }

        public int GetViewRadius { get { return viewRadius; } }

        public string GetName { get { return name; } }

        private void Locate(char[,] _map)
        {
            while (true)
            {
                x = Game._random.Next(1, _map.GetLength(1) - 2);
                y = Game._random.Next(1, _map.GetLength(0) - 2);
                if (_map[y, x] != ':')
                {
                    return;
                }
            }
        }

        private bool isWall(int derX, int derY)
        {
            if (Game._map.GetMap[y + derY, x + derX] == ':')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DoAction() // Действия игрока
        {
            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        {
                            if (!isWall(0, -1))
                            {
                                y -= 1;
                            }
                            break;
                        }
                    case ConsoleKey.PageUp:
                        {
                            if (!isWall(1, -1))
                            {
                                y -= 1;
                                x += 1;
                            }
                            break;
                        }
                    case ConsoleKey.RightArrow:
                        {
                            if (!isWall(1, 0))
                            {
                                x += 1;
                            }
                            break;
                        }
                    case ConsoleKey.PageDown:
                        {
                            if (!isWall(1, 1))
                            {
                                y += 1;
                                x += 1;
                            }
                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            if (!isWall(0, 1))
                            {
                                y += 1;
                            }
                            break;
                        }
                    case ConsoleKey.End:
                        {
                            if (!isWall(-1, 1))
                            {
                                y += 1;
                                x -= 1;
                            }
                            break;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            if (!isWall(-1, 0))
                            {
                                x -= 1;
                            }
                            break;
                        }
                    case ConsoleKey.Home:
                        {
                            if (!isWall(-1, -1))
                            {
                                y -= 1;
                                x -= 1;
                            }
                            break;
                        }
                    default: // тест смерти
                        {
                            health = health - 250 > 0 ? health - 250 : 0;
                            mana = mana - 150 > 0 ? mana - 150 : 0;
                            if (health <= 0)
                            {
                                int height;
                                int width;
                                Random Blink = new Random();
                                Console.BackgroundColor = ConsoleColor.Black;
                                Thread.Sleep(50);
                                Console.Clear();
                                while (true)
                                {
                                    if (Console.WindowHeight < 32)
                                    {
                                        Console.SetWindowSize(Console.WindowWidth, 32);
                                    }
                                    if (Console.WindowWidth < 64)
                                    {
                                        Console.SetWindowSize(64, Console.WindowHeight);
                                    }
                                    height = Console.WindowHeight / 2 - 2;
                                    width = Console.WindowWidth / 2 - 9;
                                    if (Blink.Next(0, 100) > 95)
                                    {
                                        Console.BackgroundColor = ConsoleColor.White;
                                        Console.ForegroundColor = ConsoleColor.Black;
                                    }
                                    else if (Blink.Next(0, 100) > 75)
                                    {
                                        Console.BackgroundColor = ConsoleColor.DarkRed;
                                        Console.ForegroundColor = ConsoleColor.Black;
                                    }
                                    else
                                    {
                                        Console.BackgroundColor = ConsoleColor.Black;
                                        Console.ForegroundColor = ConsoleColor.DarkRed;
                                    }
                                    Thread.Sleep(Blink.Next(100, 500));
                                    Console.Clear();
                                    Thread.Sleep(Blink.Next(50, 150));
                                    for (int i = 0; i < height; i++)
                                    {
                                        Console.Write('\n');
                                    }
                                    for (int i = 0; i < width; i++)
                                    {
                                        Console.Write(' ');
                                    }
                                    Console.Write(
                                        "  /------------\\\n");
                                    for (int i = 0; i < width; i++)
                                    {
                                        Console.Write(' ');
                                    }
                                    Console.Write(
                                        " /              \\\n");
                                    for (int i = 0; i < width; i++)
                                    {
                                        Console.Write(' ');
                                    }
                                    Console.Write(
                                        "|    YOU DIED    |\n");
                                    for (int i = 0; i < width; i++)
                                    {
                                        Console.Write(' ');
                                    }
                                    Console.Write(
                                        " \\              /\n");
                                    for (int i = 0; i < width; i++)
                                    {
                                        Console.Write(' ');
                                    }
                                    Console.Write(
                                        "  \\------------/\n");
                                }
                            }
                            break;
                        }
                }
                foreach (var m in Game._monsters)
                {
                    m.DoAction();
                }
                Game._display.RefreshScreen();
            }
        }
    }

    class Display // Отображение
    {
        private int cameraFOV;
        private char[,] mapCropped;
        private char[,] montsersMapCropped;
        private char[,] montsersMap;
        private string[,,] frame;
        private string[,,] prevFrame;
        private int frameHeight;
        private int frameWidth;
        private bool changedAgain;

        public Display(int cameraFOV)
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.SetWindowPosition(0, 0);
            Console.CursorVisible = false;
            this.cameraFOV = cameraFOV;
            Console.CursorVisible = false;
            mapCropped = new char[RenderInterface(2, false), RenderInterface(2, true)];
            montsersMap = new char[Game._map.GetMap.GetLength(0), Game._map.GetMap.GetLength(1)];
            montsersMapCropped = new char[RenderInterface(2, false), RenderInterface(2, true)];
            frame = new string[RenderInterface(1, false), RenderInterface(1, true), 3];
            prevFrame = new string[RenderInterface(1, false), RenderInterface(1, true), 3];
            frameHeight = Console.WindowHeight;
            frameWidth = Console.WindowWidth;
            changedAgain = false;
            RefreshScreen();
        }

        private int RenderInterface(int operation, bool dimension) // Расчёт размеров интерфейса: 1 - размер отображаемой карты, 2 - размер экрана 
        {
            if (operation == 1)
            {
                if (dimension)
                {
                    return Convert.ToInt32(Console.WindowWidth);
                }
                else
                {
                    return Convert.ToInt32(Console.WindowHeight) - 1;
                }
            }
            else if (operation == 2)
            {
                if (dimension)
                {
                    if (Convert.ToInt32(Console.WindowWidth * 1.00) % 2 == 0)
                    {
                        return Convert.ToInt32(Console.WindowWidth * 1.00) - 2;
                    }
                    else
                    {
                        return Convert.ToInt32(Console.WindowWidth * 1.00) - 1;
                    }
                }
                else
                {
                    if (Convert.ToInt32(Console.WindowHeight * 1.00) % 2 == 0)
                    {
                        return Convert.ToInt32(Console.WindowHeight * 1.00) - 2;
                    }
                    else
                    {
                        return Convert.ToInt32(Console.WindowHeight * 1.00) - 1;
                    }
                }
            }
            else
            {
                return 0;
            }
        }

        private string DarkenBackgroundColor(int i, int j) // Выбор цвета темнее фона
        {
            switch (mapCropped[i, j])
            {
                case ':':
                    {
                        return "Gray";
                    }
                case ';':
                    {
                        return "Black";
                    }
                case '.':
                    {
                        return "DarkGray";
                    }
                case '#':
                    {
                        return "Black";
                    }
                default:
                    {
                        return "Black";
                    }
            }
        }

        private string UseBackgroundColor(int i, int j) // Выбор цвета как у фона
        {
            switch (mapCropped[i, j])
            {
                case ':':
                    {
                        return "White";
                    }
                case ';':
                    {
                        return "DarkGray";
                    }
                case '.':
                    {
                        return "Gray";
                    }
                case '#':
                    {
                        return "Gray";
                    }
                default:
                    {
                        return "Black";
                    }
            }
        }

        private void RenderMap() // Расчёт участка карты для отображения
        {
            mapCropped = new char[RenderInterface(2, false), RenderInterface(2, true)];
            for (int i = 0; i < mapCropped.GetLength(0); i++)
            {
                for (int j = 0; j < mapCropped.GetLength(1); j++)
                {
                    if (Game._player.GetPosition[1] - ((mapCropped.GetLength(0) - 1) / 2) + i >= 0 && Game._player.GetPosition[1]
                                                  - ((mapCropped.GetLength(0) - 1) / 2) + i < Game._map.GetMap.GetLength(0) - 1 &&
                        Game._player.GetPosition[0] - ((mapCropped.GetLength(1) - 1) / 2) + j >= 0 && Game._player.GetPosition[0]
                                                  - ((mapCropped.GetLength(1) - 1) / 2) + j < Game._map.GetMap.GetLength(1) - 1)
                    {
                        mapCropped[i, j] = Game._map.GetMap[Game._player.GetPosition[1] - ((mapCropped.GetLength(0) - 1) / 2) + i,
                                                            Game._player.GetPosition[0] - ((mapCropped.GetLength(1) - 1) / 2) + j];
                    }
                    else
                    {
                        mapCropped[i, j] = '#';
                    }
                }
            }
        }

        private void RenderMonsters() // Расчёт персонажей на участке карты для отображения
        {
            montsersMap = new char[Game._map.GetMap.GetLength(0), Game._map.GetMap.GetLength(1)];
            for (int i = 0; i < montsersMap.GetLength(0); i++)
            {
                for (int j = 0; j < montsersMap.GetLength(1); j++)
                {
                    montsersMap[i, j] = '#';
                    foreach (var m in Game._monsters)
                    {
                        if (i == m.GetPosition[1] && j == m.GetPosition[0])
                        {
                            montsersMap[i, j] = m.GetIcon;
                            break;
                        }
                    }
                }
            }
            montsersMapCropped = new char[RenderInterface(2, false), RenderInterface(2, true)];
            for (int i = 0; i < montsersMapCropped.GetLength(0); i++)
            {
                for (int j = 0; j < montsersMapCropped.GetLength(1); j++)
                {
                    if (Game._player.GetPosition[1] - ((montsersMapCropped.GetLength(0) - 1) / 2) + i >= 0 && Game._player.GetPosition[1]
                                                  - ((montsersMapCropped.GetLength(0) - 1) / 2) + i < montsersMap.GetLength(0) - 1 &&
                        Game._player.GetPosition[0] - ((montsersMapCropped.GetLength(1) - 1) / 2) + j >= 0 && Game._player.GetPosition[0]
                                                  - ((montsersMapCropped.GetLength(1) - 1) / 2) + j < montsersMap.GetLength(1) - 1)
                    {
                        montsersMapCropped[i, j] = montsersMap[Game._player.GetPosition[1] - ((montsersMapCropped.GetLength(0) - 1) / 2) + i,
                                                            Game._player.GetPosition[0] - ((montsersMapCropped.GetLength(1) - 1) / 2) + j];
                    }
                    else
                    {
                        montsersMapCropped[i, j] = '#';
                    }
                }
            }
        }

        private void Render3D() // Расчёт "3D" графики
        {
            for (int i = 0; i < mapCropped.GetLength(0); i++)
            {
                for (int j = 0; j < mapCropped.GetLength(1); j++)
                {
                    if (mapCropped[i, j] == ':')
                    {
                        if (i < (mapCropped.GetLength(0) - 1) / 2 - cameraFOV - 1 && j > (mapCropped.GetLength(1) - 1) / 2 - cameraFOV - 1 && j < (mapCropped.GetLength(1) - 1) / 2 + cameraFOV + 2)
                        {
                            if (mapCropped[i + 1, j] != ':')
                            {
                                mapCropped[i + 1, j] = ';';
                            }
                        }
                        if (i < (mapCropped.GetLength(0) - 1) / 2 - cameraFOV - 1 && j > (mapCropped.GetLength(1) - 1) / 2 + cameraFOV + 1)
                        {
                            if (mapCropped[i, j - 1] != ':')
                            {
                                mapCropped[i, j - 1] = ';';
                            }
                            if (mapCropped[i + 1, j - 1] != ':')
                            {
                                mapCropped[i + 1, j - 1] = ';';
                            }
                            if (mapCropped[i + 1, j] != ':')
                            {
                                mapCropped[i + 1, j] = ';';
                            }
                        }
                        if (i > (mapCropped.GetLength(0) - 1) / 2 - cameraFOV - 2 && i < (mapCropped.GetLength(0) - 1) / 2 + cameraFOV + 1 && j > (mapCropped.GetLength(1) - 1) / 2 + cameraFOV + 1)
                        {
                            if (mapCropped[i, j - 1] != ':')
                            {
                                mapCropped[i, j - 1] = ';';
                            }
                        }
                        if (i > (mapCropped.GetLength(0) - 1) / 2 + cameraFOV && j > (mapCropped.GetLength(1) - 1) / 2 + cameraFOV + 1)
                        {
                            if (mapCropped[i, j - 1] != ':')
                            {
                                mapCropped[i, j - 1] = ';';
                            }
                            if (mapCropped[i - 1, j - 1] != ':')
                            {
                                mapCropped[i - 1, j - 1] = ';';
                            }
                            if (mapCropped[i - 1, j] != ':')
                            {
                                mapCropped[i - 1, j] = ';';
                            }
                        }
                        if (i > (mapCropped.GetLength(0) - 1) / 2 + cameraFOV && j > (mapCropped.GetLength(1) - 1) / 2 - cameraFOV - 1 && j < (mapCropped.GetLength(1) - 1) / 2 + cameraFOV + 2)
                        {
                            if (mapCropped[i - 1, j] != ':')
                            {
                                mapCropped[i - 1, j] = ';';
                            }
                        }
                        if (i > (mapCropped.GetLength(0) - 1) / 2 + cameraFOV && j < (mapCropped.GetLength(1) - 1) / 2 - cameraFOV)
                        {
                            if (mapCropped[i, j + 1] != ':')
                            {
                                mapCropped[i, j + 1] = ';';
                            }
                            if (mapCropped[i - 1, j + 1] != ':')
                            {
                                mapCropped[i - 1, j + 1] = ';';
                            }
                            if (mapCropped[i - 1, j] != ':')
                            {
                                mapCropped[i - 1, j] = ';';
                            }
                        }
                        if (i > (mapCropped.GetLength(0) - 1) / 2 - cameraFOV - 2 && i < (mapCropped.GetLength(0) - 1) / 2 + cameraFOV + 1 && j < (mapCropped.GetLength(1) - 1) / 2 - cameraFOV)
                        {
                            if (mapCropped[i, j + 1] != ':')
                            {
                                mapCropped[i, j + 1] = ';';
                            }
                        }
                        if (i < (mapCropped.GetLength(0) - 1) / 2 - cameraFOV - 1 && j < (mapCropped.GetLength(1) - 1) / 2 - cameraFOV)
                        {
                            if (mapCropped[i, j + 1] != ':')
                            {
                                mapCropped[i, j + 1] = ';';
                            }
                            if (mapCropped[i + 1, j + 1] != ':')
                            {
                                mapCropped[i + 1, j + 1] = ';';
                            }
                            if (mapCropped[i + 1, j] != ':')
                            {
                                mapCropped[i + 1, j] = ';';
                            }
                        }
                    }
                }
            }
        }

        private void RenderScreen() // Расчёт всего экрана и занесение его в матрицу
        {
            frame = new string[RenderInterface(1, false), RenderInterface(1, true), 3]; // 1 - цвет фона, 2 - цвет шрифта, 3 - символ
            for (int i = 0; i < frame.GetLength(0); i++)
            {
                for (int j = 0; j < frame.GetLength(1); j++)
                {
                    frame[i, j, 0] = "Black";
                    frame[i, j, 1] = "White";
                    frame[i, j, 2] = " ";
                }
            }
            RenderMap();
            RenderMonsters();
            Render3D();
            int index = 0;
            for (int i = 0; i < mapCropped.GetLength(0); i++) // Отображение карты уровня и монстров
            {
                for (int j = 0; j < mapCropped.GetLength(1); j++)
                {
                    if (mapCropped[i, j] == ':')
                    {
                        frame[i, j, 0] = "White";
                        frame[i, j, 1] = "White";
                        frame[i, j, 2] = " ";
                    }
                    else if (mapCropped[i, j] == ';')
                    {
                        frame[i, j, 0] = "DarkGray";
                        frame[i, j, 1] = "White";
                        frame[i, j, 2] = " ";
                    }
                    else if (mapCropped[i, j] == '.')
                    {
                        frame[i, j, 0] = "Gray";
                        frame[i, j, 1] = "White";
                        frame[i, j, 2] = " ";
                    }
                    else if (mapCropped[i, j] == '#')
                    {
                        frame[i, j, 0] = "Black";
                        frame[i, j, 1] = "White";
                        frame[i, j, 2] = " ";
                    }
                    if (montsersMapCropped[i, j] != '#')
                    {
                        frame[i, j, 0] = UseBackgroundColor(i, j);
                        frame[i, j, 1] = "Red";
                        frame[i, j, 2] = montsersMapCropped[i, j].ToString();
                    }
                }
            }
            frame[(mapCropped.GetLength(0) - 1) / 2, (mapCropped.GetLength(1) - 1) / 2, 0] = UseBackgroundColor  // Отображение игрока
                 ((mapCropped.GetLength(0) - 1) / 2, (mapCropped.GetLength(1) - 1) / 2);
            frame[(mapCropped.GetLength(0) - 1) / 2, (mapCropped.GetLength(1) - 1) / 2, 1] = "Green";
            frame[(mapCropped.GetLength(0) - 1) / 2, (mapCropped.GetLength(1) - 1) / 2, 2] = Game._player.GetIcon.ToString();
            string temp = Game._player.GetName; // Отображение имени        НУЖНО ДОБАВИТЬ УЧЕТ ПЕРЕНОСА КАРТЫ В ЦЕНТР ВО ВСЕХ МЕСТАХ ИНТРФЕЙСА И ИГРОКА (ЦВЕТ ФОНА)
            for (int i = 0; i < temp.Length; i++)
            {
                frame[1, i + 1, 0] = DarkenBackgroundColor(1, i + 1);
                frame[1, i + 1, 1] = "White";
                frame[1, i + 1, 2] = temp[i].ToString();
            }
            temp = "           /"; // Отображение здоровья
            for (int i = 0; i < temp.Length; i++)
            {
                frame[2, i + 1, 0] = DarkenBackgroundColor(2, i + 1);
                frame[2, i + 1, 1] = "White";
                frame[2, i + 1, 2] = temp[i].ToString();
                index++;
            }
            index++;
            for (double k = 0; k < Game._player.GetMaxHealth; k += Game._player.GetMaxHealth * 0.027f)
            {
                if (Game._player.GetHealth > k)
                {
                    if (Game._player.GetHealth >= Game._player.GetMaxHealth * 0.75f)
                    {
                        frame[2, index, 0] = "Green";
                        frame[2, index, 1] = "White";
                        frame[2, index, 2] = " ";
                    }
                    else if (Game._player.GetHealth >= Game._player.GetMaxHealth * 0.15f)
                    {
                        frame[2, index, 0] = "Yellow";
                        frame[2, index, 1] = "White";
                        frame[2, index, 2] = " ";
                    }
                    else
                    {
                        frame[2, index, 0] = "Red";
                        frame[2, index, 1] = "White";
                        frame[2, index, 2] = " ";
                    }
                }
                else
                {
                    frame[2, index, 0] = DarkenBackgroundColor(2, index);
                    frame[2, index, 1] = "White";
                    frame[2, index, 2] = " ";
                }
                index++;
            }
            temp = @"\ ";
            for (int i = 0; i < temp.Length; i++)
            {
                frame[2, i + index, 0] = DarkenBackgroundColor(2, i + index);
                frame[2, i + index, 1] = "White";
                frame[2, i + index, 2] = temp[i].ToString();
            }
            index = 1;
            if (Convert.ToInt32(Game._player.GetHealth) < 10)
            {
                temp = "HP " + Convert.ToInt32(Game._player.GetHealth) + "      <";
                for (int i = 0; i < temp.Length; i++)
                {
                    frame[3, i + 1, 0] = DarkenBackgroundColor(3, i + 1);
                    frame[3, i + 1, 1] = "White";
                    frame[3, i + 1, 2] = temp[i].ToString();
                    index++;
                }
            }
            else if (Convert.ToInt32(Game._player.GetHealth) < 100)
            {
                temp = "HP " + Convert.ToInt32(Game._player.GetHealth) + "     <";
                for (int i = 0; i < temp.Length; i++)
                {
                    frame[3, i + 1, 0] = DarkenBackgroundColor(3, i + 1);
                    frame[3, i + 1, 1] = "White";
                    frame[3, i + 1, 2] = temp[i].ToString();
                    index++;
                }
            }
            else if (Convert.ToInt32(Game._player.GetHealth) < 1000)
            {
                temp = "HP " + Convert.ToInt32(Game._player.GetHealth) + "    <";
                for (int i = 0; i < temp.Length; i++)
                {
                    frame[3, i + 1, 0] = DarkenBackgroundColor(3, i + 1);
                    frame[3, i + 1, 1] = "White";
                    frame[3, i + 1, 2] = temp[i].ToString();
                    index++;
                }
            }
            else if (Convert.ToInt32(Game._player.GetHealth) < 10000)
            {
                temp = "HP " + Convert.ToInt32(Game._player.GetHealth) + "   <";
                for (int i = 0; i < temp.Length; i++)
                {
                    frame[3, i + 1, 0] = DarkenBackgroundColor(3, i + 1);
                    frame[3, i + 1, 1] = "White";
                    frame[3, i + 1, 2] = temp[i].ToString();
                    index++;
                }
            }
            else if (Convert.ToInt32(Game._player.GetHealth) < 100000)
            {
                temp = "HP " + Convert.ToInt32(Game._player.GetHealth) + "  <";
                for (int i = 0; i < temp.Length; i++)
                {
                    frame[3, i + 1, 0] = DarkenBackgroundColor(3, i + 1);
                    frame[3, i + 1, 1] = "White";
                    frame[3, i + 1, 2] = temp[i].ToString();
                    index++;
                }
            }
            else
            {
                temp = "HP " + Convert.ToInt32(Game._player.GetHealth) + " <";
                for (int i = 0; i < temp.Length; i++)
                {
                    frame[3, i + 1, 0] = DarkenBackgroundColor(3, i + 1);
                    frame[3, i + 1, 1] = "White";
                    frame[3, i + 1, 2] = temp[i].ToString();
                    index++;
                }
            }
            for (double k = 0; k < Game._player.GetMaxHealth; k += Game._player.GetMaxHealth * 0.025f)
            {
                if (Game._player.GetHealth > k)
                {
                    if (Game._player.GetHealth >= Game._player.GetMaxHealth * 0.75f)
                    {
                        frame[3, index, 0] = "Green";
                        frame[3, index, 1] = "White";
                        frame[3, index, 2] = " ";
                    }
                    else if (Game._player.GetHealth >= Game._player.GetMaxHealth * 0.15f)
                    {
                        frame[3, index, 0] = "Yellow";
                        frame[3, index, 1] = "White";
                        frame[3, index, 2] = " ";
                    }
                    else
                    {
                        frame[3, index, 0] = "Red";
                        frame[3, index, 1] = "White";
                        frame[3, index, 2] = " ";
                    }
                }
                else
                {
                    frame[3, index, 0] = DarkenBackgroundColor(3, index);
                    frame[3, index, 1] = "White";
                    frame[3, index, 2] = " ";
                }
                index++;
            }
            frame[3, index, 0] = DarkenBackgroundColor(3, index);
            frame[3, index, 1] = "White";
            frame[3, index, 2] = ">";
            temp = @"           \";
            index = 1;
            for (int i = 0; i < temp.Length; i++)
            {
                frame[4, i + 1, 0] = DarkenBackgroundColor(4, index);
                frame[4, i + 1, 1] = "White";
                frame[4, i + 1, 2] = temp[i].ToString();
                index++;
            }
            for (double k = 0; k < Game._player.GetMaxHealth; k += Game._player.GetMaxHealth * 0.027f)
            {
                if (Game._player.GetHealth > k)
                {
                    if (Game._player.GetHealth >= Game._player.GetMaxHealth * 0.75f)
                    {
                        frame[4, index, 0] = "Green";
                        frame[4, index, 1] = "White";
                        frame[4, index, 2] = " ";
                    }
                    else if (Game._player.GetHealth >= Game._player.GetMaxHealth * 0.15f)
                    {
                        frame[4, index, 0] = "Yellow";
                        frame[4, index, 1] = "White";
                        frame[4, index, 2] = " ";
                    }
                    else
                    {
                        frame[4, index, 0] = "Red";
                        frame[4, index, 1] = "White";
                        frame[4, index, 2] = " ";
                    }
                }
                else
                {
                    frame[4, index, 0] = DarkenBackgroundColor(4, index);
                    frame[4, index, 1] = "White";
                    frame[4, index, 2] = " ";
                }
                index++;
            }
            temp = "/ ";
            for (int i = 0; i < temp.Length; i++)
            {
                frame[4, i + index, 0] = DarkenBackgroundColor(4, i + index);
                frame[4, i + index, 1] = "White";
                frame[4, i + index, 2] = temp[i].ToString();
            }
            index = 1;
            if (Convert.ToInt32(Game._player.GetMana) < 10) // Отображение маны
            {
                temp = "MP " + Convert.ToInt32(Game._player.GetMana) + "       <";
                for (int i = 0; i < temp.Length; i++)
                {
                    frame[5, i + 1, 0] = DarkenBackgroundColor(5, i + 1);
                    frame[5, i + 1, 1] = "White";
                    frame[5, i + 1, 2] = temp[i].ToString();
                    index++;
                }
            }
            else if (Convert.ToInt32(Game._player.GetMana) < 100)
            {
                temp = "MP " + Convert.ToInt32(Game._player.GetMana) + "      <";
                for (int i = 0; i < temp.Length; i++)
                {
                    frame[5, i + 1, 0] = DarkenBackgroundColor(5, i + 1);
                    frame[5, i + 1, 1] = "White";
                    frame[5, i + 1, 2] = temp[i].ToString();
                    index++;
                }
            }
            else if (Convert.ToInt32(Game._player.GetMana) < 1000)
            {
                temp = "MP " + Convert.ToInt32(Game._player.GetMana) + "     <";
                for (int i = 0; i < temp.Length; i++)
                {
                    frame[5, i + 1, 0] = DarkenBackgroundColor(5, i + 1);
                    frame[5, i + 1, 1] = "White";
                    frame[5, i + 1, 2] = temp[i].ToString();
                    index++;
                }
            }
            else if (Convert.ToInt32(Game._player.GetMana) < 10000)
            {
                temp = "MP " + Convert.ToInt32(Game._player.GetMana) + "    <";
                for (int i = 0; i < temp.Length; i++)
                {
                    frame[5, i + 1, 0] = DarkenBackgroundColor(5, i + 1);
                    frame[5, i + 1, 1] = "White";
                    frame[5, i + 1, 2] = temp[i].ToString();
                    index++;
                }
            }
            else if (Convert.ToInt32(Game._player.GetMana) < 100000)
            {
                temp = "MP " + Convert.ToInt32(Game._player.GetMana) + "   <";
                for (int i = 0; i < temp.Length; i++)
                {
                    frame[5, i + 1, 0] = DarkenBackgroundColor(5, i + 1);
                    frame[5, i + 1, 1] = "White";
                    frame[5, i + 1, 2] = temp[i].ToString();
                    index++;
                }
            }
            else
            {
                temp = "MP " + Convert.ToInt32(Game._player.GetMana) + "  <";
                for (int i = 0; i < temp.Length; i++)
                {
                    frame[5, i + 1, 0] = DarkenBackgroundColor(5, i + 1);
                    frame[5, i + 1, 1] = "White";
                    frame[5, i + 1, 2] = temp[i].ToString();
                    index++;
                }
            }
            for (double k = 0; k < Game._player.GetMaxMana; k += Game._player.GetMaxMana * 0.035f)
            {
                if (Game._player.GetMana > k)
                {
                    if (Game._player.GetMana >= Game._player.GetMaxMana * 0.15f)
                    {

                        frame[5, index, 0] = "Cyan";
                        frame[5, index, 1] = "White";
                        frame[5, index, 2] = " ";
                    }
                    else
                    {
                        frame[5, index, 0] = "Blue";
                        frame[5, index, 1] = "White";
                        frame[5, index, 2] = " ";
                    }
                }
                else
                {
                    frame[5, index, 0] = DarkenBackgroundColor(5, index);
                    frame[5, index, 1] = "White";
                    frame[5, index, 2] = " ";
                }
                index++;
            }
            frame[5, index, 0] = DarkenBackgroundColor(5, index);
            frame[5, index, 1] = "White";
            frame[5, index, 2] = ">";
        }

        public void RefreshScreen() // Вывод изменённых пикселей
        {
            if (Console.WindowHeight < 32)
            {
                Console.SetWindowSize(frameWidth, 32);
                changedAgain = true;
            }
            if (Console.WindowWidth < 64)
            {
                Console.SetWindowSize(64, frameHeight);
                changedAgain = true;
            }
            Console.ResetColor();
            Console.SetCursorPosition(0, 0);
            if (frameHeight != Console.WindowHeight || frameWidth != Console.WindowWidth || changedAgain == true)
            {
                changedAgain = false;
                Console.Clear();
                frame = new string[RenderInterface(1, false), RenderInterface(1, true), 3];
                prevFrame = new string[RenderInterface(1, false), RenderInterface(1, true), 3];
                frameHeight = Console.WindowHeight;
                frameWidth = Console.WindowWidth;
            }
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            prevFrame = frame; // Запоминание предыдущего состояния экрана
            RenderScreen();
            for (int i = 0; i < frame.GetLength(0); i++)
            {
                for (int j = 0; j < frame.GetLength(1); j++)
                {
                    if (frame[i, j, 0] == prevFrame[i, j, 0] && frame[i, j, 1] == prevFrame[i, j, 1] && frame[i, j, 2] == prevFrame[i, j, 2])
                    {
                        continue;
                    }
                    Console.SetCursorPosition(j, i);
                    switch (frame[i, j, 0])
                    {
                        case "Black":
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                                break;
                            }
                        case "Blue":
                            {
                                Console.BackgroundColor = ConsoleColor.Blue;
                                break;
                            }
                        case "Cyan":
                            {
                                Console.BackgroundColor = ConsoleColor.Cyan;
                                break;
                            }
                        case "DarkBlue":
                            {
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                                break;
                            }
                        case "DarkCyan":
                            {
                                Console.BackgroundColor = ConsoleColor.DarkCyan;
                                break;
                            }
                        case "DarkGray":
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                                break;
                            }
                        case "DarkGreen":
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGreen;
                                break;
                            }
                        case "DarkMagenta":
                            {
                                Console.BackgroundColor = ConsoleColor.DarkMagenta;
                                break;
                            }
                        case "DarkRed":
                            {
                                Console.BackgroundColor = ConsoleColor.DarkRed;
                                break;
                            }
                        case "DarkYellow":
                            {
                                Console.BackgroundColor = ConsoleColor.DarkYellow;
                                break;
                            }
                        case "Gray":
                            {
                                Console.BackgroundColor = ConsoleColor.Gray;
                                break;
                            }
                        case "Green":
                            {
                                Console.BackgroundColor = ConsoleColor.Green;
                                break;
                            }
                        case "Magenta":
                            {
                                Console.BackgroundColor = ConsoleColor.Magenta;
                                break;
                            }
                        case "Red":
                            {
                                Console.BackgroundColor = ConsoleColor.Red;
                                break;
                            }
                        case "White":
                            {
                                Console.BackgroundColor = ConsoleColor.White;
                                break;
                            }
                        case "Yellow":
                            {
                                Console.BackgroundColor = ConsoleColor.Yellow;
                                break;
                            }
                        default:
                            {
                                Console.ResetColor();
                                break;
                            }
                    }
                    switch (frame[i, j, 1])
                    {
                        case "Black":
                            {
                                Console.ForegroundColor = ConsoleColor.Black;
                                break;
                            }
                        case "Blue":
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                            }
                        case "Cyan":
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                break;
                            }
                        case "DarkBlue":
                            {
                                Console.ForegroundColor = ConsoleColor.DarkBlue;
                                break;
                            }
                        case "DarkCyan":
                            {
                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                                break;
                            }
                        case "DarkGray":
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                break;
                            }
                        case "DarkGreen":
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                break;
                            }
                        case "DarkMagenta":
                            {
                                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                                break;
                            }
                        case "DarkRed":
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                break;
                            }
                        case "DarkYellow":
                            {
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                break;
                            }
                        case "Gray":
                            {
                                Console.ForegroundColor = ConsoleColor.Gray;
                                break;
                            }
                        case "Green":
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            }
                        case "Magenta":
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                break;
                            }
                        case "Red":
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            }
                        case "White":
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        case "Yellow":
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                            }
                        default:
                            {
                                Console.ResetColor();
                                break;
                            }
                    }
                    Console.Write(frame[i, j, 2]);
                }
            }
        }
    }

    class Game // Содержит Main
    {
        public static Random _random;
        public static Map _map;
        public static Display _display;
        public static Player _player;
        public static List<Monster> _monsters;

        public static void Main()
        {
            _random = new Random();
            _map = new Map(256);
            _player = new Player("Nate Zephyr", 'N', 2500, 1500, 10); // Максимум Хп и Мп - 999999
            _monsters = new List<Monster>();
            for (int i = 0; i < 100; i++)
            {
                _monsters.Add(new Monster("Quzmin", 'Q', 350, 100));
            }
            _display = new Display(5);
        }
    }
}
