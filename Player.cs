using System;
using System.Collections.Generic;
using System.Text;

namespace HW6
{
    class Player
    {
        public void Start()
        {
            Console.WriteLine("Welcome to my RPG world");
            bool login = false;
            bool endGame = false;
            string account = "";
            while (!login && !endGame)      //尚未登入遊戲
            {
                Console.WriteLine("這裡是登入選單\n請輸入執行指令\t0:註冊\t1:登入\t其他鍵:離開遊戲");      //登入選單
                switch (Console.ReadLine())
                {
                    case "0":
                        Register();
                        break;
                    case "1":
                        Login(ref account, ref login);
                        break;
                    default:
                        Console.WriteLine("您要離開遊戲嗎\nY:確認\tN:返回");
                        switch (Console.ReadLine().ToUpper())
                        {
                            case "Y":
                                Console.WriteLine("遊戲結束");
                                endGame = true;
                                break;
                            case "N":
                                Console.WriteLine("已返回");
                                break;
                            default:
                                Console.WriteLine("指令錯誤，已返回");
                                break;
                        }
                        break;
                }
            }
            bool select = false;
            int heroNo = 0;
            while (login && !endGame)
            {
                if (!select)
                {
                    heroNo = SelectHero(account,ref endGame);
                    select = true;
                }
                GameManager.Gm.RunGame(ref GameManager.Gm.Users[account].hero[heroNo], ref select);
            }
        }   //開啟遊戲
        void Register() //註冊
        {
            bool register = false;
            while (!register)
            {
                Console.WriteLine("請輸入要建立的帳號");
                string account = Console.ReadLine();
                Console.WriteLine("請輸入要設定的密碼(請注意大小寫不同)");
                string password = Console.ReadLine();
                Console.WriteLine($"建立帳號為:{account}\n設定密碼為{password}，確定請按Y，按其他任意按鍵以重新輸入");
                switch (Console.ReadLine().ToUpper())
                {
                    case "Y":
                        if (GameManager.Gm.Users.ContainsKey(account))
                        {
                            Console.WriteLine("帳號重複，請重新輸入");
                        }
                        else
                        {
                            GameManager.User user = new GameManager.User();
                            user.password = password;
                            user.hero = new Hero[3];    //產生3格的英雄槽
                            GameManager.Gm.Users.Add(account, user);
                            Console.WriteLine("註冊成功，請登入遊戲");
                            register = true;
                        }
                        break;
                    default:
                        Console.WriteLine("請重新輸入");
                        break;
                }
            }
        }
        void Login(ref string account,ref bool login) //登入
        {
            bool exit = false;
            while (!login)
            {
                Console.WriteLine("請輸入登入帳號");
                account = Console.ReadLine();
                Console.WriteLine("請輸入登入密碼(請注意大小寫不同)");
                string password = Console.ReadLine();
                CheckLogin(account, password, ref login,ref exit);
                if (exit) { break; }
            }
        }
        void CheckLogin(string account, string password, ref bool login,ref bool exit)   //檢查帳號密碼
        {
            if (GameManager.Gm.Users.ContainsKey(account))
            {
                string correctPw = GameManager.Gm.Users[account].password;
                if (password == correctPw)
                {
                    Console.WriteLine("密碼正確，成功登入遊戲");
                    login = true;
                }
                else
                {
                    Console.WriteLine("密碼錯誤，請重新輸入");
                }
            }
            else
            {
                Console.WriteLine("帳號不存在，請先進行註冊");
                exit = true;
            }
        }
        int SelectHero(string account, ref bool endGame)   //選擇英雄(創建或登入遊戲)
        {
            Console.WriteLine("請輸入對應的英雄欄位(1~3)，選擇英雄進入遊戲，或選擇空欄位創建英雄，或按 X 以結束遊戲");
            for (int i = 0; i<3; i++)
            {
                if (GameManager.Gm.Users[account].hero[i] == null)
                {
                    Console.WriteLine($"英雄欄位{i + 1} : 未創建");
                }
                else
                {
                    Console.WriteLine($"英雄欄位{i + 1} : {GameManager.Gm.Users[account].hero[i].name}");
                }
            }
            int selectNo = 4;
            while ((selectNo >3) | (selectNo <0))
            {
                string input = Console.ReadLine().ToUpper();
                if (input == "X")
                {
                    Console.WriteLine("結束遊戲");
                    endGame = true;
                }
                else
                {
                    try
                    {
                        selectNo = (Convert.ToInt32(input) - 1);
                        bool check = false;
                        while (check == false)
                        {
                            Console.WriteLine($"您選擇的號碼為{selectNo + 1}，確認按Y，按其他鍵以重選");
                            if (Console.ReadLine().ToUpper() == "Y")
                            {
                                check = true;
                            }
                            else
                            {
                                Console.WriteLine("請輸入對應的英雄欄位(1~3)，選擇以創建的英雄進入遊戲，或空欄位創建英雄");
                                selectNo = (Convert.ToInt32(Console.ReadLine()) - 1);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("輸入錯誤，請重新輸入");
                        Console.WriteLine("請輸入對應的英雄欄位(1~3)，選擇以創建的英雄進入遊戲，或空欄位創建英雄");
                        selectNo = (Convert.ToInt32(Console.ReadLine()) - 1);
                    }
                }
            }
            Hero hero = new Hero();
            if (GameManager.Gm.Users[account].hero[selectNo] == null)
            {
                GameManager.Gm.Users[account].hero[selectNo] = hero.CreateHero();
                Console.WriteLine("已創建英雄");
                GameManager.Gm.Users[account].hero[selectNo].ShowStatus();
            }
            else
            {
                Console.WriteLine("已選定英雄");
                GameManager.Gm.Users[account].hero[selectNo].ShowStatus();
            }
            return selectNo;
        }
    }
}
