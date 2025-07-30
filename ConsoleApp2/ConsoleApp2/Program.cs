using System.Diagnostics;

namespace ConsoleApp2
{
    internal class Program
    {
        public static bool overway = false;
        public static void Lose(Warrior pl)
        {
            if (pl.isalive == false)
            {
                Console.WriteLine("Поражение");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        public static void Chose(Warrior pl, Warrior en, List<Item> inv)//изменить
        {
            Console.WriteLine(" 1 - статус \n 2 - атака \n 3 - поставить блок \n 4 - инвентарь ");
            string a = Console.ReadLine();
            switch (a)
            {
                case "1": { Console.WriteLine("Вы"); status(pl); Console.WriteLine("Враг"); status(en); Console.WriteLine("Нажмите любую клавишу для продолжения"); Console.ReadKey(); Console.Clear(); break; }
                case "2": { Attack(pl, en); overway = true; break; }
                case "3": { pl.inblock = true; overway = true; break; }
                case "4": {  openinv(pl, inv); break;  }
                default: Console.Clear(); break;

            }
        }
        static void openinv(Warrior pl, List<Item> inv)
        {
            // Вывод
            for (int i = 0; i < inv.Count; i++) {
                Console.WriteLine($"{i+1} - использовать {inv[i].name} (исцеление - {inv[i].heal})");
            }
            Console.WriteLine("0 - закрыть инвентарь");
            int a = Convert.ToInt32(Console.ReadLine());
            if (a == 0) { return; }
            for (int i = 0; i < inv.Count; i++)
            {
                if (a == (i + 1)) { inv[i].useitem(pl); inv.Remove(inv[i]); return; }
            }
            //Выбор
        }
        public static void Block(Warrior defend, Warrior attack)
        {
            Random a = new Random();
            int b = a.Next(1, 100);
            if (defend.block == 0)
            {
                Console.WriteLine($"Не удалось блокировать");
                defend.inblock = false;
                Attack(attack, defend);
                return;
            }
            if(defend.block/2 > b)
            {
                Console.WriteLine($"{defend.name} парировал удар, нанесенный {attack.name}");
                Attack(defend, attack);
                defend.inblock = false;
                return;
            }
            if (defend.block > b)
            {
                Console.WriteLine($"{defend.name} блокирывал удар, нанесенный {attack.name}"); defend.inblock = false; return;
            }
            else
            {
                Console.WriteLine($"Не удалось блокировать");
                defend.inblock = false;
                Attack(attack, defend);
                return;
            }
        }
        public static void Attack(Warrior attack, Warrior defend)
        {
            if (attack.isalive == false || defend.isalive == false) { return; }
            if (defend.inblock == true) { Block(defend,attack); return; }
            if ((attack.dm - (defend.sh - (defend.sh * attack.br / 100))) <= 0) { Console.WriteLine("Не бробил защиту"); return;}
            defend.hp = defend.hp - (attack.dm - (defend.sh - defend.sh * attack.br / 100));
            Console.WriteLine($"{attack.name} атаковал {defend.name} на {attack.dm - (defend.sh - defend.sh*attack.br/100)} едениц урона с помощью - {attack.wearpon}");
            defend.IsDeath();
        }
        public static void status(Warrior warrior)
        {
            Console.WriteLine("----------------------------");
            if (warrior.isalive == false)
            {
                Console.WriteLine(warrior.name + " Мертв");
            }
            else
            {
                Console.WriteLine("СТАТУС");
                Console.WriteLine("Имя " + warrior.name);
                Console.WriteLine("Здоровье " + warrior.hp);
                Console.WriteLine("Защита " + warrior.sh);
                Console.WriteLine("Шанс блокирования " + warrior.block + " %");
                Console.WriteLine("Деньги " + warrior.money);
                Console.WriteLine("Оружие " + warrior.wearpon);
                Console.WriteLine("Урон " + warrior.dm);
                Console.WriteLine("Пробитие " + warrior.br + " %");
            }
            Console.WriteLine("----------------------------");
        }
        static void fight(Warrior warrior1, Warrior warrior2,List<Item> inv)
        {
            Console.WriteLine($"На {warrior1.name} напал {warrior2.name}");
            for (; warrior2.isalive == true && warrior1.isalive == true;)
            {
                Chose(warrior1, warrior2,inv);
                if(overway == true) { Attack(warrior2, warrior1);overway = false; }
                if (warrior2.isalive == false) { warrior1.money += warrior2.money; };
                Console.WriteLine("----------------------");
            }
            Lose(warrior1);
            overway = false;
            Random a = new Random();
            int b = a.Next(0, 3);
            if (b == 2) { Drop(warrior2, warrior1, inv); }
            Console.WriteLine($"{warrior1.name} победил {warrior2.name}!");
            Console.WriteLine("Нажмите любую клавишу для продолжения"); Console.ReadKey(); Console.Clear();
        }
        public static void MultiChose(Warrior pl, Warrior[] en , List<Item> inv)//изменить
        {
            Console.WriteLine(" 1 - статус \n 2 - атака \n 3 - блок \n 4 - инвентарь");
            string a = Console.ReadLine();
            switch (a)
            {
                case "1":
                    {
                        Console.WriteLine("Вы"); status(pl);
                        for (int i = 0; i < en.Length; i++)
                        {
                            Console.WriteLine("Враг");
                            status(en[i]);
                        };
                        Console.WriteLine("Нажмите любую клавишу для продолжения"); Console.ReadKey(); Console.Clear();
                        break;
                    }
                case "2":
                    {
                        for (int i = 0; i < en.Length; i++)
                        {
                            Attack(pl, en[i]); 
                        }
                        overway = true;
                        break;
                    }
                case "3":
                    { pl.inblock = true; overway = true; break; }
                case "4":
                    { openinv(pl, inv);break; }
                default: Console.Clear(); break;

            }
        }
            static void multifight(Warrior pl, Warrior[] en, List<Item> inv)
            {
                bool multilive = true;
                for (; pl.isalive == true && multilive == true;)
                {
                    MultiChose(pl, en, inv);
                    if (overway == true)
                    {
                        for (int i = 0; i < en.Length; i++)
                        {
                            Attack(en[i], pl); 
                        }
                        overway = false;
                        Console.WriteLine("----------------------");
                    }
                    for (int i = 0; i < en.Length; i++)
                    {
                        if (en[i].isalive == false) { multilive = false; }
                        if (en[i].isalive == true) { multilive = true; break; }
                    }
                }
                Lose(pl);
            for (int i = 0; i < en.Length; i++)
            {
                pl.money += en[i].money;
                Random a = new Random();
                int b = a.Next(0, 3);
                if (b == 2) { Drop(en[i], pl, inv); }
            }
            Console.WriteLine($"{pl.name} победил");
            }
        public static void Shop(Item[] products,Warrior pl, List<Item> inv)
        {
            //Console.WriteLine($"1 - Купить предмет {products[0].name}\n 2 - Купить предмет {products[1].name} \n 3 - Купить предмет {products[2].name} \n 4 - Купить предмет {products[3].name}\n Нажмите любую клавишу чтобы очистить историю");
            for(int i=0;i<products.Length; i++)
            {
                if (products[i].type == 2) { Console.WriteLine($"{i + 1} - купить {products[i].name} (урон - {products[i].dm} ; пробитие - {products[i].br};) цена - {products[i].cost}"); }
                if (products[i].type == 1) { Console.WriteLine($"{i + 1} - купить {products[i].name} (исцеление - {products[i].heal};) цена - {products[i].cost}"); }
                if (products[i].type == 3) { Console.WriteLine($"{i + 1} - купить {products[i].name} (шанс блокирования - {products[i].heal};) цена - {products[i].cost}"); }
                if (products[i].type == 4) { Console.WriteLine($"{i + 1} - купить {products[i].name} (защита - {products[i].heal};) цена - {products[i].cost}"); }
            }
            string a = Console.ReadLine();
            switch (a)
            {
                case "1": { getitem(products[0],pl,inv); overway = true; break; }
                case "2": { getitem(products[1], pl, inv); overway = true; break; }
                case "3": { getitem(products[2], pl, inv); overway = true; break; }
                case "4": { getitem(products[3], pl, inv); overway = true; break; }
                default: Console.Clear(); break;

            }
        }
        public static void Prival(Warrior pl, Item[] products, List<Item> inv)
        {
            for (; overway == false;)
            {
                Console.WriteLine(" 1 - статус \n 2 - привал \n 3 - магазин \n 4 - инвентарь \n ");
                string a = Console.ReadLine();
                switch (a)
                {
                    case "1": { Console.WriteLine("Вы"); status(pl); Console.WriteLine("Нажмите любую клавишу для продолжения"); Console.ReadKey(); Console.Clear(); break; }
                    case "2": { pl.hp += 10; Console.WriteLine($"{pl.name} исцелен на 10"); overway = true; break; }
                    case "3": { Shop(products, pl, inv);  break; }
                    case "4": { openinv(pl, inv); break; }
                    default: Console.Clear(); break;

                }
            }
            overway = false;

        }
        public static void getitem(Item item, Warrior pl, List<Item> inv)
        {
            if (pl.money >= item.cost)
            {
                if (item.type == 1) { inv.Add(item); return; }
                if (item.type == 2) { item.useitem(pl); return; }
                if (item.type == 3) { item.useitem(pl); return; }
                if (item.type == 4) { item.useitem(pl); return; }
                pl.money -= item.cost;
            }
            else
            {
                Console.WriteLine($"{pl.name} слишком нищий");
            }
        }
        public static void Drop(Warrior en, Warrior pl,List<Item> inv)
        {
            if (en.drop.type == 1) { Console.WriteLine($"Вы подобрали {en.drop.name}");  inv.Add(en.drop); return; }
            if (en.drop.type == 2) {
                status(pl);
                Console.WriteLine($"Враг роняет {en.drop.name} (урон - {en.drop.dm} пробитие - {en.drop.br}) \n1 - подобрать \n2 - выкинкть"); 
                string inp = Console.ReadLine();
                if (inp == "1") { en.drop.useitem(pl); return; }
                else { return; }
            }
            if (en.drop.type == 3) {
                status(pl);
                Console.WriteLine($"Враг роняет {en.drop.name} (Шанс блокирывания - {en.drop.heal}) \n1 - подобрать \n2 - выкинкть");
                string inp = Console.ReadLine();
                if (inp == "1") { en.drop.useitem(pl); return; }
                else { return; }
            }
            if (en.drop.type == 4) {
                status(pl);
                Console.WriteLine($"Враг роняет {en.drop.name} (Защита - {en.drop.heal}) \n 1 - подобрать \n2 - выкинкть");
                string inp = Console.ReadLine();
                if (inp == "1") { en.drop.useitem(pl); return; }
                else { return; }
            }
        }

        /// ///////////////////////////////////////////////////

        static void Main(string[] args)
        {
        ////////////////////////////////////////////////////////////////////////////////////////////  
                Item shild = new Item("Щит Новичка",3,10,65,0,0);
                Item armor = new Item("Броня Воина", 4, 7, 10, 0, 0);
                Item apple = new Item("Яблоко", 1, 5, 2, 0, 0);
                Item bulova = new Item("Булова", 2, 5, 0, 8, 60);
                Item pineaple = new Item("Ананас", 1, 5, 3, 0, 0);
                Item[] items = { apple, armor, shild, bulova };

            Item pilum = new Item("Пилум", 2, 15, 0, 11, 75);
            Item pie = new Item("Пирог", 1, 7, 8, 0, 0);
            Item shild2 = new Item("Широкий Щит", 3, 12, 85, 0, 0);
            Item pel = new Item("Пельмени", 1, 11, 15, 0, 0);
            Item[] shop1 = { pilum,pie,shild2,pineaple};
            List<Item> inv = new List<Item> { apple,pineaple};

            Item spear = new Item("Копьё", 2, 20, 0, 20, 70);

            /////////////////////////////////////////////////////////////////////////
            Warrior warrior2 = new Warrior("Гоблин", 10, 5, 0, 10, " Кинжал ", 5, 50);
            warrior2.drop = pilum;
            Warrior warrior3 = new Warrior("Гоблин", 5, 2, 0, 5, " Кинжал ", 2, 60);
            warrior3.drop = pel;
            Warrior warrior4 = new Warrior("Гоблин - воин", 25, 2, 0, 5, " Кинжал ", 3, 75);
            warrior4.drop = pel;
            Warrior warrior5 = new Warrior("Гоблин", 5, 2, 0, 5, " Кинжал ", 2, 60);
            warrior5.drop = pel;
            Warrior ogr = new Warrior("Огр", 30, 5, 0, 15, "Кулак", 12, 20);
            ogr.drop = pie;
            Warrior[] goblingang = { warrior3, warrior4, warrior5 };
            Warrior wor1 = new Warrior("Разбойник",15,0,0,7,"Ржавый клинок",10,15);
            wor1.drop = pie;
            Warrior wor2 = new Warrior("Разбойник", 15, 0, 0, 9, "Длинное копьё", 20, 70);
            wor2.drop = spear;
            Warrior[] wors = { wor1, wor2 };
            Warrior war = new Warrior("Дворф",10,50,0,20,"Боевой Молот",18,25);
            ////////////////////////////////////////////////////////////////////////////////////////////  
            Console.Write("Введите имя: ");
            string nickname = Console.ReadLine();
            Warrior pl = new Warrior(nickname, 10, 5, 20, 0," Ржавый длинный меч " , 5,  50);
            fight(pl, warrior2, inv);
            Prival(pl, items, inv);
            Console.WriteLine("На вас напала банда гоблинов");
            multifight(pl, goblingang, inv);
            Prival(pl, shop1, inv);
            fight(pl, ogr, inv);
            Console.WriteLine("Вы встретили бомжа, он просит дать ему 2 монеты\n 1 - Дать 2 монеты\n 2 - Отказать");
            string chose = Console.ReadLine();
            if (chose == "1" && pl.money >= 2) { Console.WriteLine("Бомж благодарит вас"); pl.money -= 2; } else { Console.WriteLine($"Бомж вас проклял и на {pl.wearpon} появились трещины (-2 урона)"); pl.dm -= 2; }
            Prival(pl, shop1, inv);
            Console.WriteLine("На вас напали воры и требуют у вас 20 монеты\n 1 - Дать 20 монеты\n 2 - Отказать");
            chose = Console.ReadLine();
            if (chose == "1" && pl.money >= 20) { Console.WriteLine("Разбойники отпутили вас");pl.money -= 20; } else {Console.WriteLine("Разбойникик напали на "+ pl.name); multifight(pl, wors, inv); }
            Prival(pl, shop1, inv);
            Console.WriteLine("Победа");
            Console.ReadKey();
        }
        /// ///////////////////////////////////////////////////
    }
    public class Warrior
    {
        // Юнит
        public string? name { get; set; }
        public double hp { get; set; }
        public double sh { get; set; }
        public int money { get; set; }
        public int block { get; set; }
        public bool inblock {  get; set; }
        public bool isalive { get; set; } = true;
        // Оружие
        public string wearpon { get; set; }
        public double dm { get; set; }
        public double br {get; set;}
        //противники
        public Item drop {  get; set; }


        public Warrior(string? _name, double hp, double sh, int block,int money,string werapon, double dm, double br)
        {
            this.name = _name;
            this.hp = hp;
            this.sh = sh;
            this.block = block;
            this.money = money;
            this.wearpon = werapon;
            this.dm = dm;
            this.br = br;
        }
        public void IsDeath() {
            if (hp <= 0) 
            {
                Console.WriteLine(name+" Мертв");
                isalive = false;
            }
        }
    }

    public class Item
    {
        public string? name { get; set; }
        public int type{ get; set; } //1 - хил 2 - оружие 3 - щиты
        public int cost { get; set; }
        // для хила
        public int heal { get; set; }
        // для оружия 
        public double dm { get; set; }
        public double br {get; set;}

        public Item(string? name, int type, int cost, int heal, double dm, double br)
        {
            this.name = name;
            this.type = type;
            this.cost = cost;
            this.heal = heal;
            this.dm = dm;
            this.br = br;
        }
        public void useitem(Warrior pl)
        {
                if (type == 1) { pl.hp += heal; Console.WriteLine($"Игрок исцелен с помощью {name} на {heal} ед здоровья"); }
                if (type == 2) { pl.wearpon = name; pl.dm = dm; pl.br = br; ; Console.WriteLine($"Игрок подобрал {name}"); }
                if (type == 3) { pl.block = heal; Console.WriteLine($"Игрок подобрал {name} ({heal} %  шанс блокировки)"); }
                if (type == 4) { pl.sh = heal; Console.WriteLine($"Игрок подобрал {name} ({heal} ед защиты)"); }
        }
        
    }
}
