using Newtonsoft.Json.Linq;

namespace TextRPG_Week3
{
    public class ShopItem
    {
        public List<ShopItem> ItemList { get; set; } = new List<ShopItem>();

        public Item ItemData { get; set; }
        public int Price { get; set; }
        public bool IsPurchased { get; set; } = false;
        public ShopItem() { }
        public string GetDisplayInfo(int index)
        {
            string statText = ItemData.Type switch
            {
                ItemType.Weapon => $"공격력 +{ItemData.Value}",
                ItemType.Armor => $"방어력 +{ItemData.Value}",
                ItemType.HealthPotion => $"체력 회복 +{ItemData.Value}",
                ItemType.ManaPotion => $"마나 회복 + {ItemData.Value}",
                _ => ""
            };

            string rightText = IsPurchased && ItemData.IsConsumable
                ? "구매완료"
                : $"{Price} G";

            return $"- {index + 1}. {ItemData.Name,-12} | {statText,-14} | {ItemData.Description,-30} | {rightText}";
        }
        //GetDisplayInfo함수(정수)
        //타입에 따라 아이템 값이 추가해주는 능력치 문자열로 변환
        
        //아이템을 구매했다면 구매완료, 아니면 가격
        
        //문자열 반환

        public void InitShopItems()
        {
            ItemList = new List<ShopItem>
            {
                // 무기
                new ShopItem
                {
                    ItemData = new Item("낡은 검", ItemType.Weapon, 5, "오래된 검이지만 쓸만함"),
                    Price = 300
                },
                new ShopItem
                {
                    ItemData = new Item("검", ItemType.Weapon, 7, "잘 제련된 양산형 검"),
                    Price = 500
                },
                new ShopItem
                {
                    ItemData = new Item("강철 검", ItemType.Weapon, 10, "강철로 제련한 튼튼한 검"),
                    Price = 700
                },
                new ShopItem
                {
                    ItemData = new Item("낡은 스파르타의 창", ItemType.Weapon, 20, "오래된 스파르타의 창"),
                    Price = 1000
                },
                new ShopItem
                {
                    ItemData = new Item("스파르타의 창", ItemType.Weapon, 30, "강인한 스파르타 인들의 무기"),
                    Price = 1500
                },
                new ShopItem
                {
                    ItemData = new Item("스파르타의 혼", ItemType.Weapon, 2500, "고대 스파르타 영웅들의 혼이 담긴 에고웨폰"),
                    Price = 10000
                },

                // 방어구
                new ShopItem
                {
                    ItemData = new Item("가죽 갑옷", ItemType.Armor, 3, "얇은 가죽으로 만들어진 기본 방어구입니다."),
                    Price = 250
                },
                new ShopItem
                {
                    ItemData = new Item("강화 가죽 갑옷", ItemType.Armor, 5, "추가 패딩으로 보호력이 향상된 가죽 방어구입니다."),
                    Price = 500
                },
                new ShopItem
                {
                    ItemData = new Item("청동 갑옷", ItemType.Armor, 7, "기초 금속으로 제작된 무난한 방어구입니다."),
                    Price = 700
                },
                new ShopItem
                {
                    ItemData = new Item("무쇠 갑옷", ItemType.Armor, 10, "무쇠로 제작되어 튼튼하고 신뢰할 수 있는 방어구입니다."),
                    Price = 1000
                },
                new ShopItem
                {
                    ItemData = new Item("사슬 갑옷", ItemType.Armor, 15, "금속 사슬로 엮어 찌르기에 강한 방어구입니다."),
                    Price = 1300
                },
                new ShopItem
                {
                    ItemData = new Item("강철 갑옷", ItemType.Armor, 20, "강철로 만들어진 중갑. 뛰어난 내구성을 자랑합니다."),
                    Price = 1700
                },
                new ShopItem
                {
                    ItemData = new Item("중갑 기사단 갑옷", ItemType.Armor, 30, "중갑 기사단이 착용하던 신뢰도 높은 갑옷입니다."),
                    Price = 2000
                },
                new ShopItem
                {
                    ItemData = new Item("마법 금속 갑옷", ItemType.Armor, 40, "마법이 부여된 금속 방어구로 물리와 마법을 함께 방어합니다."),
                    Price = 2500
                },
                new ShopItem
                {
                    ItemData = new Item("용비늘 갑옷", ItemType.Armor, 50, "전설 속 용의 비늘을 정제해 만든 강력한 방어구입니다."),
                    Price = 3000
                },
                new ShopItem
                {
                    ItemData = new Item("스파르타 갑옷", ItemType.Armor, 2500, "스파르타 최정예 전사들이 착용했던 전설적인 갑옷입니다."),
                    Price = 10000
                },

                // 소비 아이템
                new ShopItem
                {
                    ItemData = new Item("회복 포션", ItemType.HealthPotion, 30, "체력을 30 회복합니다", isConsumable: true),
                    Price = 100
                },
                new ShopItem
                {
                    ItemData = new Item("마나 포션", ItemType.ManaPotion, 30, "마나를 30 회복합니다", isConsumable: true),
                    Price = 100
                }
            };
        }
        //InitShopItems함수
        //아이템 추가

        public void OpenShop(Character player)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("상점");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"보유 골드: {player.Gold} G\n");

                for (int i = 0; i < ItemList.Count; i++)
                {
                    switch (ItemList[i].ItemData.Type)
                    {
                        case ItemType.Weapon:
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            break;
                        case ItemType.Armor:
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            break;
                        case ItemType.HealthPotion:
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case ItemType.ManaPotion:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        default:
                            Console.ResetColor();
                            break;
                    }
                    Console.WriteLine(ItemList[i].GetDisplayInfo(i));
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n0: 나가기");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("아이템 번호를 입력하여 구매하세요.");
                Console.Write(">> ");
                Console.ResetColor();
                string input = Console.ReadLine();

                if (input == "0") break;

                if (int.TryParse(input, out int selected) && selected >= 1 && selected <= ItemList.Count)
                {
                    Buy(player, ItemList[selected - 1]);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadKey();
                }
            }
        }
        //OpenShop함수
        //상점 정보 표시
        //아이템갯수만큼 반복
        //-아이템 종류에 따라
        //--무기 = 짙은빨강
        //--방어구 = 짙은파랑
        //--회복포션 = 빨강
        //--마나포션 = 파랑
        //--그외(없음) 초기색상
        
        //-아이템 정보 표시
        
        //메세지 출력
        //값 입력받기
        //"0"이면 나가기
        //숫자로 변환 가능하고 아이템 번호와 일치하면
        //-Buy함수에 플레이어, 해당아이템 매개변수 호출
        //아니면 잘못입력 출력 후 반복

        private static void Buy(Character player, ShopItem item)
        {
            if (player.Gold < item.Price)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("골드가 부족합니다.");
                Console.ReadKey();
                return;
            }
            ItemType type = item.ItemData.Type;
            if(type == ItemType.HealthPotion || type == ItemType.ManaPotion)
            {
                Item invenPotion = player.Inventory.FirstOrDefault(item => item.Type == type);
                if (invenPotion != null && item.ItemData.Type == invenPotion.Type) invenPotion.Count++;
            }
            else
            {
                if (item.IsPurchased)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("이미 구매한 아이템입니다.");
                    Console.ReadKey();
                    return;
                }
                player.Inventory.Add(new Item(
                item.ItemData.Name,
                item.ItemData.Type,
                item.ItemData.Value,
                item.ItemData.Description
                ));
            }
            player.Gold -= item.Price;



            if (!item.ItemData.IsConsumable) item.IsPurchased = true;

            player.Inventory = player.Inventory
            .OrderBy(item => item.Type)
            .ThenBy(item => item.Value)
            .ThenBy(item => item.Name.Length)
            .ToList();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{item.ItemData.Name}을(를) 구매했습니다!");
            Console.ReadKey();
        }
        //Buy함수(플레이어, 상점아이템)
        //소지금이 가격보다 낮으면 부족하다 출력 후 반환
        //아이템 타입 저장
        //회복포션이나 마나포션일 때
        //-해당 아이템과 인벤토리에 같은 타입의 아이템이 있을때 저장
        //-아이템이 있다면 갯수 추가
        
        //아니면
        //이미 구매한 아이템일때
        //-이미구매함 표시 후 반환
        //아이템 추가
        
        //소지금 - 해당아이템 가격
        //소모품이 아니면 구매함 true
        
        //인벤토리 정렬
        
        //구매메세지 출력 후 반환
    }
}
