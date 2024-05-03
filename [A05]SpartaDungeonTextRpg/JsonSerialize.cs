using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpartaDungeonTextRpg;

namespace _A05_SpartaDungeonTextRpg
{
    // 상단의 도구 탭 - NuGet 패키지 관리자 - 패키지 관리자 콘솔 클릭
    // PM> 부분에 Install-Package Newtonsoft.Json 작성 후 엔터를 입력해 패키지 다운
    // 또는
    // 상단의 프로젝트 탭 - NuGet 패키지 관리 - 찾아보기 - Newtonsoft.Json 다운

    public class JsonSerialize
    {
        public static GameManager gameManager;

        public static void SaveData(Player player, Item item, Potion potion, Quest quest)
        {
            ItemData itemData; // 지역변수
            PlayerData playerData;
            PotionData potionData;
            QuestSave questSave;

            Console.Clear();

            // 저장할 파일명 지정
            string fileName = "playerData.json";
            string itemFileName = "itemData.json";
            string potionFileName = "potionData.json";
            string questFileName = "questSave.json";

            // 데이터 경로 저장 (C드라이브, Documents)
            string userDocumentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = Path.Combine(userDocumentsFolder, fileName);
            string itemFilePath = Path.Combine(userDocumentsFolder, itemFileName);
            string potionFilePath = Path.Combine(userDocumentsFolder, potionFileName);
            string questFilePath = Path.Combine(userDocumentsFolder, questFileName);

            // 데이터들을 각 값에 맞는 곳으로 전달
            playerData = new PlayerData(player);
            itemData = new ItemData(item);
            potionData = new PotionData(potion);
            questSave = new QuestSave(quest);
            
            // 데이터 저장 (직렬화)
            string playerJson = JsonConvert.SerializeObject(playerData, Formatting.Indented);
            string itemJson = JsonConvert.SerializeObject(itemData, Formatting.Indented);
            string potionJson = JsonConvert.SerializeObject(potionData, Formatting.Indented);
            string questJson = JsonConvert.SerializeObject(questSave, Formatting.Indented);

            File.WriteAllText(filePath, playerJson);
            File.WriteAllText(itemFilePath, itemJson);
            File.WriteAllText(potionFilePath, potionJson);
            File.WriteAllText(questFilePath, questJson);

            Console.WriteLine("저장이 완료되었습니다.");
            Console.WriteLine("메인 메뉴로 돌아갑니다.");
            Thread.Sleep(1000);
            gameManager.MainMenu();
        }

        public static void LoadData(GameManager gm, Player player, Item item, Potion potion, Quest quest)
        {
            gameManager = gm;
            ItemData itemData;
            PlayerData playerData;
            PotionData potionData;
            QuestSave questSave;

            Console.Clear();

            // 불러올 파일명 지정
            string fileName = "playerData.json";
            string itemFileName = "itemData.json";
            string potionFileName = "potionData.json";
            string questFileName = "questSave.json";

            // 데이저 경로 불러오기 (C드라이브, Documents)
            string userDocumentsFolder = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
            
            // 플레이어 데이터 로드
            string playerFilePlth = Path.Combine(userDocumentsFolder, fileName);
            if (File.Exists(playerFilePlth))
            {
                string playerJson = File.ReadAllText(playerFilePlth);
                playerData = JsonConvert.DeserializeObject<PlayerData>(playerJson); // 역직렬화
                player.SetPlayer(playerData); // SetPlayer 함수로 player에게 값 전달
                
                Console.WriteLine("플레이어 정보를 불러왔습니다.");
                Thread.Sleep(1000);
            }
            else
            {
                Console.WriteLine("저장된 플레이어 데이터가 없습니다.");
                Thread.Sleep(1000);
                gameManager.PlayerName(); // 이름, 직업 설정
            }

            Console.Clear();

            // 포션 데이터 로드
            string potionFilePath = Path.Combine(userDocumentsFolder, potionFileName);
            if (File.Exists(potionFilePath))
            {
                string potionJson = File.ReadAllText(potionFilePath);
                potionData = JsonConvert.DeserializeObject<PotionData>(potionJson); // 역직렬화
                potion.SetPotion(potionData); // SetPotion 함수로 값 전달
                
                Console.WriteLine("빨간약과 파란약을 챙겼습니다.");
                Thread.Sleep(1000);
            }
            else
            {
                Console.WriteLine("물약 제조중...");
                Thread.Sleep(1000);
            }

            // 퀘스트 데이터 로드
            string questFilePath = Path.Combine(userDocumentsFolder, questFileName);
            if (File.Exists(questFilePath))
            {
                string questJson = File.ReadAllText(questFilePath);
                questSave = JsonConvert.DeserializeObject<QuestSave>(questJson); // 역직렬화
                quest.SetQuest(questSave); // SetQuest 함수로 값 전달

                Console.WriteLine("의뢰 진행상황을 가져왔습니다.");
                Thread.Sleep(1000);
            }
            else
            {
                Console.WriteLine("모험가 협회 신규 의뢰서를 받았습니다.");
                Thread.Sleep(1000);
            }

            //아이템 데이터 로드
            string itemFilePath = Path.Combine(userDocumentsFolder, itemFileName);
            if (File.Exists(itemFilePath))
            {
                string itemJson = File.ReadAllText(itemFilePath);
                itemData = JsonConvert.DeserializeObject<ItemData>(itemJson); // 역직렬화
                item.SetItem(itemData); // 아이템 리스트 생성 후 데이터 입력

                Console.WriteLine("여관에서 가방을 가져왔습니다.");
                Thread.Sleep(1000);

                gameManager.MainMenu();
            }
            else
            {
                Console.WriteLine("저장된 아이템 데이터가 없습니다.");
                Thread.Sleep(1000);
                Console.WriteLine("모험가용 가방을 얻었습니다.");
                Thread.Sleep(1000);
                Console.WriteLine("상점에 새로운 물건들이 들어옵니다.");
                Thread.Sleep(2000);

                gameManager.MainMenu();
            }
        }
    }
}
