    using System;
    using System.Collections.Generic;

    class Program
    {
        //=================================================
            // Escolhas de Profissão
        static bool andarilho = false;
        static bool mercenario = false;
        static bool paladino = false;
        static bool vigilante = false;

        // Escolhas de Raça
        static bool zoot = false;
        static bool bioAndroid = false;
        static bool humano = false;
        static bool superMutante = false;

        // Define apenas os limites máximos no topo
        const int VIDA_MAXIMA = 215;
        const int FOME_MAXIMA = 100;
        const int SEDE_MAXIMA = 100;
        const int DEFESA_MAXIMA = 100;
        const int ESTAMINA_MAXIMA = 20;

        // --- DESBLOQUEIO DOS NÍVEIS ---
        static bool nivel2Desbloqueado = false;
        static bool nivel3Desbloqueado = false;
        static bool nivel4Desbloqueado = false;
        static bool nivel5Desbloqueado = false;

        // --- OBJETIVOS DO NÍVEL 1 ---
        static bool achouChave = false;
        static bool achouArquivos = false;
        static bool achouProjetoPurificador = false;
        static bool ajudouMercador = false;
        static bool achouInformações = false;
        static bool completeGame = false;
        static bool achouArquivosSecretos = false;
        static bool salvarPessoasdoBunker = false;
        static bool serSequestrado = false;
        static bool bossMorto = false;
        static bool escapar = false;
        static bool completarVinganca = false;

        // Moeda para a Loja 
        static int scrapCoins = 50; 

        // Itens da loja:
        static int racao = 0;
        static int garrafadeAgua = 0;
        static int energetico = 0;
        static int kitMedico = 0;
        static int bandagem = 0;
        static bool colete = false;
        static bool armaduradeCouro = false;
        static bool armaduraMilitarReforcada = false;
        static bool armaduraMilitar = false;
        static bool manopola = false;
        static bool socoIngles = false;
        static bool luvas = false;
        static bool RifledePrecisão = false;
        static bool SniperAWM = false;
        static bool eagle = false;
        static bool glock = false;
        static bool marchadoPesado = false;
        static bool marchadoTatico = false;
        static bool cutelo = false;
        static bool m4A1 = false;
        static bool miniGun = false;
        static bool maconha = false;
        static bool panodePrato = false;
        static bool roupaRasgada = false;
        static bool pneuFurado = false;
        // Mini Boss
        static int vidaMiniBoss = 300;
        static int danoMiniBoss = 25;
        static bool miniBossDerrotado = false;

        static string[] premiosGacha = new string[]
            {
                "Maconha", 
                "Pano de Prato", 
                "Roupa Rasgada", 
                "Pneu Furado", 
                "MiniGun", 
                "Armadura Militar Reforçada"
            };
        static string[] nomesItensLoja = new string[] 
        {
            "Ração",                    "Garrafa de Água",   "Energético",       "Kit Médico",        "Bandagem",
            "Colete",                   "Armadura de Couro", "Armadura Mil. Ref","Armadura Militar",  "Manopla",
            "Soco Inglês",              "Luvas",             "Rifle de Precisão","Sniper AWM",        "Eagle",
            "Glock",                    "Machado Pesado",    "Machado Tático",   "Cutelo",            "M4A1",
            "MiniGun",                  "Maconha",           "Pano de Prato",    "Roupa Rasgada",     "Caixa Gacha"
        };

        static int[] precosItensLoja = new int[]
        {
        15, 10, 12, 35, 8,  // LIN 1: Ração(15), Água(10), Energético(12), Kit Médico(35), Bandagem(8)
        20, 30, 200, 100, 40, // LIN 2: Colete(20), Armadura Couro(30), Armadura Ref(200), Armadura Mil(100), Manopla(40)
        25, 18, 70, 90, 35, // LIN 3: Soco Inglês(25), Luvas(18), Rifle(70), Sniper(90), Eagle(35)
        25, 60, 35, 30, 55, // LIN 4: Glock(25), Machado P.(60), Machado T.(35), Cutelo(30), M4A1(55)
        99, 15, 5,  10, 25  // LIN 5: MiniGun(99), Maconha(15), Pano de Prato(5), Roupa Rasgada(10), Gacha(25)
        };

        // Status do RPG 
        static int defesaP; 
        static int vidaP;
        static int estaminaP;
        static int dano;
        static int forca;        
        static int defesaBonus;
        static int danoBonus;
        static int forcaBonus;
        static int sede = 100;
        static int fome = 100;

        //Estátisca de dano/fome/sede
        static int DefesaTotal()
        {
            return Math.Clamp(defesaP + defesaBonus, 0, DEFESA_MAXIMA);
        }

        static int ForcaTotal()
        {
            return Math.Clamp(forca + forcaBonus, 0, 100);
        }

        static int DanoTotal()
        {
            return Math.Clamp(dano + danoBonus + ForcaTotal(), 0, 200);
        }
        static DateTime proximaReducaoStatus;
        //=================================================

        // Dados do personagem
        static string playerName = "";
        static string playerRace = "";
        static string playerClass = "";

        // Controle do jogo
        static bool gameRunning = true;
        static int currentRoom = 0;
        static Random random = new Random();

        // Estruturas obrigatórias
        static List<string> inventory = new List<string>();

        static Dictionary<int, string> rooms = new Dictionary<int, string>();

        //=================================================
        // MAIN
        //=================================================

        static void Main()
        {
            InitializeGame();
        }

        //=================================================
        // INICIALIZAÇÃO
        //=================================================

        static void InitializeGame()
        {
            LoadGameData();

            bool startGame = MainMenu();

            if (!startGame)
            {  
                CloseGame();
                return;
            }
            gameRunning = true;
            CreateCharacter();
            MenuNiveis();
            CloseGame();
        }


        //=================================================
        // MENU PRINCIPAL
        //=================================================

        static bool MainMenu()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("==================================");
                Console.WriteLine("          ATOMIC SURVIVAL -demo-");
                Console.WriteLine("==================================");
                Console.WriteLine("1 - Novo Jogo");
                Console.WriteLine("2 - Créditos");
                Console.WriteLine("3 - Sair");
                Console.Write("\nEscolha: ");

                if (!int.TryParse(Console.ReadLine(), out int option))
                    continue;

                switch (option)
                {
                    case 1:
                        return true;

                    case 2:
                        Credits();
                        break;

                    case 3:
                        return false;
                }
            }
        }

        //=================================================
        // LOOP PRINCIPAL
        //=================================================

        static void GameLoop()
        {
            while (gameRunning)
            {
                ShowHUD();

                ShowCurrentRoom();

                if (!gameRunning)
                    break;

                PlayerAction();

                if (!gameRunning)
                    break;

                UpdateGame();

                gameRunning = !CheckGameOver();
            }

            if (CheckGameOver())
                ShowEnding();
        }

        //=================================================
        // CARREGAMENTO DOS DADOS
        //=================================================

        static void LoadGameData()
        {
            // Carregar salas

            // Carregar itens

            // Carregar inimigos

            // Configurações iniciais
        }

        //=================================================
        // PERSONAGEM
        //=================================================

        static void CreateCharacter()
        {
            Console.Clear();
            Console.WriteLine("==================================");
            Console.WriteLine("      CRIAÇÃO DE PERSONAGEM       ");
            Console.WriteLine("==================================");

            Console.Write("Nome do Player: ");
            playerName = Console.ReadLine();

            // 1. SELEÇÃO DE RAÇA
            while (true)
            {
                Console.WriteLine("\nEscolha sua Raça:");
                Console.WriteLine("1 - Zoot (vida: 50)");
                Console.WriteLine("2 - Humano (vida: 100)");
                Console.WriteLine("3 - BioAndroid (vida: 110)");
                Console.WriteLine("4 - Super Mutante (vida: 150)");
                Console.Write("Escolha (Digite o número): ");
                string opcaoRaca = Console.ReadLine();

                bool racaValida = true;

                switch (opcaoRaca)
                {
                    case "1":
                        zoot = true;
                        playerRace = "Zoot";
                        vidaP = 50;
                        break;
                    case "2":
                        humano = true;
                        playerRace = "Humano";
                        vidaP = 100;
                        break;
                    case "3":
                        bioAndroid = true;
                        playerRace = "BioAndroid";
                        vidaP = 110;
                        break;
                    case "4":
                        superMutante = true;
                        playerRace = "Super Mutante";
                        vidaP = 150;
                        break;
                    default:
                        Console.WriteLine("\nOpção inválida! Escolha um número de 1 a 4.");
                        racaValida = false;
                        break;
                }

                if (racaValida) break; 
            }

            // 2. SELEÇÃO DE PROFISSÃO
            while (true)
            {
                Console.WriteLine("\nEscolha sua Profissão:");
                Console.WriteLine("1 - Andarilho - Força: 5; Defesa: 5; Estamina: 10.");
                Console.WriteLine("2 - Mercenário - Força: 8; Defesa: 8; Estamina: 9.");
                Console.WriteLine("3 - Paladino - Força: 9; Defesa: 15; Estamina: 6.");
                Console.WriteLine("4 - Vigilante - Força: 6; Defesa: 8; Estamina: 15.");
                Console.Write("Escolha (Digite o número): ");
                string opcaoClasse = Console.ReadLine();

                bool profissaoValida = true;

                switch (opcaoClasse)
                {
                    case "1":
                        andarilho = true;
                        playerClass = "Andarilho";
                        forca = 5;
                        dano = 5;
                        defesaP = 5;
                        estaminaP = 10;
                        break;
                    case "2":
                        mercenario = true;
                        playerClass = "Mercenário";
                        forca = 8;
                        dano = 5;
                        defesaP = 8;
                        estaminaP = 9;
                        break;
                    case "3":
                        paladino = true;
                        playerClass = "Paladino";
                        forca = 9;
                        dano = 5;
                        defesaP = 15;
                        estaminaP = 6;
                        break;
                    case "4":
                        vigilante = true;
                        playerClass = "Vigilante";
                        forca = 6;
                        dano = 5;
                        defesaP = 8;
                        estaminaP = 15;
                        break;
                    default:
                        Console.WriteLine("\nOpção inválida! Escolha um número de 1 a 4.");
                        profissaoValida = false;
                        break;
                }

                if (profissaoValida) Console.Clear(); break; 
                
            }

            // Exibição do resultado
            Console.Clear();
            Console.WriteLine("==================================");
            Console.WriteLine("       REVISÃO DO PERSONAGEM      ");
            Console.WriteLine("==================================");
            Console.WriteLine($"Nome: {playerName}");
            Console.WriteLine($"Raça: {playerRace}");
            Console.WriteLine($"Classe: {playerClass}");
            Console.WriteLine("----------------------------------");
            Console.WriteLine($"Vida Inicial: {vidaP}");
            Console.WriteLine($"Fome Inicial: {fome}");
            Console.WriteLine($"Sede Inicial: {sede}");
            Console.WriteLine($"Força Inicial: {forca}");
            Console.WriteLine($"Defesa Inicial: {defesaP}");
            Console.WriteLine($"Estamina Inicial: {estaminaP}");
            Console.WriteLine("==================================");
            Console.WriteLine("\nCriação concluída com sucesso!");
            Console.WriteLine("\n==Observação: você perde fome e sede!!==");
            proximaReducaoStatus = DateTime.Now.AddSeconds(10);
            Console.ReadKey();
            Console.WriteLine("Pressione qualquer tecla para continuar!");
            Console.Clear();
        }
        static void MenuNiveis()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==================================");
                Console.WriteLine("        SELEÇÃO DE NÍVEIS         ");
                Console.WriteLine("==================================");
                Console.WriteLine("1 - Nível 1 [ABERTO]");
                Console.WriteLine("2 - Nível 2 " + (nivel2Desbloqueado ? "[ABERTO]" : "[TRANCADO]"));
                Console.WriteLine("3 - Nível 3 " + (nivel3Desbloqueado ? "[ABERTO]" : "[TRANCADO]"));
                Console.WriteLine("4 - Nível 4 " + (nivel4Desbloqueado ? "[ABERTO]" : "[TRANCADO]"));
                Console.WriteLine("5 - Nível 5 " + (nivel5Desbloqueado ? "[ABERTO]" : "[TRANCADO]"));
                Console.WriteLine("----------------------------------");
                Console.WriteLine("L - Acessar Loja");
                Console.WriteLine("M - Voltar para o Menu Principal");
                Console.Write("\nEscolha para onde ir: ");

                string escolha = Console.ReadLine()?.ToUpper() ?? "";
                if (escolha == "1")
                {
                    currentRoom = 0; // Entrada do Nível 1
                    ExibirMensagemClasseAoEntrar(1);
                    gameRunning = true;
                    GameLoop();

                    if (CheckGameOver())
                        break;
                }
                else if (escolha == "2")
                {
                    if (nivel2Desbloqueado)
                    {
                        currentRoom = 10; // Entrada do Nível 2
                        ExibirMensagemClasseAoEntrar(2);
                        gameRunning = true;
                        GameLoop();

                        if (CheckGameOver())
                            break;
                    }
                    else
                    {
                        Console.WriteLine("\n[TRANCADO] Você ainda não cumpriu os objetivos da sua classe no Nível 1!");
                        Console.ReadKey();
                    }
                }
                else if (escolha == "3")
                {
                    if (nivel3Desbloqueado)
                    {
                        achouChave = true;
                        achouArquivos = true;
                        achouProjetoPurificador = true;
                        ajudouMercador = true;
                        achouInformações = true;
                        completeGame = true;
                        achouArquivosSecretos = true;
                        salvarPessoasdoBunker = true;
                        serSequestrado = true;
                        bossMorto = true;
                        escapar = true;
                        completarVinganca = true;

                        currentRoom = 20;
                        gameRunning = false;
                        ShowEnding();
                        return;
                                       /* ExibirMensagemClasseAoEntrar(3);
                                        gameRunning = true;
                                        GameLoop();

                                        if (CheckGameOver())
                                            break;  */
                    }
                    else
                    {
                        Console.WriteLine("\n[TRANCADO] Complete o Nível 2 primeiro!");
                        Console.ReadKey();
                    }
                }
                else if (escolha == "4")
                {
                    if (nivel4Desbloqueado)
                    {
                        currentRoom = 30;
                        ExibirMensagemClasseAoEntrar(4);
                        gameRunning = true;
                        GameLoop();

                        if (CheckGameOver())
                            break;
                    }
                    else
                    {
                        Console.WriteLine("\n[TRANCADO] Complete o Nível 3 primeiro!");
                        Console.ReadKey();
                    }
                }
                else if (escolha == "5")
                {
                    if (nivel5Desbloqueado)
                    {
                        currentRoom = 40;
                        ExibirMensagemClasseAoEntrar(5);
                        gameRunning = true;
                        GameLoop();

                        if (CheckGameOver())
                            break;
                    }
                    else
                    {
                        Console.WriteLine("\n[TRANCADO] Complete o Nível 4 primeiro!");
                        Console.ReadKey();
                    }
                }
                else if (escolha == "L")
                {
                    AbrirLoja();
                }
                else if (escolha == "M")
                {
                    break;
                }
            }
        }
        //Loja de Itens para serem comprados.
        static void AbrirLoja()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=========================================================================");
                Console.WriteLine("                         LOJA DE SUCATA (SCRAP)                          ");
                Console.WriteLine("=========================================================================");
                Console.WriteLine($" Suas Moedas: {scrapCoins} ScrapCoins");
                Console.WriteLine("-------------------------------------------------------------------------");
                Console.WriteLine("         |   COLUNA 1        |   COLUNA 2             |   COLUNA 3              |   COLUNA 4          |   COLUNA 5   |");
                Console.WriteLine("-------------------------------------------------------------------------");
                Console.WriteLine($"LIN 1 | {nomesItensLoja[0]}(15 SC) | {nomesItensLoja[1]}(10 SC) | {nomesItensLoja[2]}(12 SC) | {nomesItensLoja[3]}(35 SC) | {nomesItensLoja[4]}(8 SC) |");
                Console.WriteLine($"LIN 2 | {nomesItensLoja[5]}(20 SC) | {nomesItensLoja[6]}(30 SC) | {nomesItensLoja[7]}(200 SC) | {nomesItensLoja[8]}(100 SC) | {nomesItensLoja[9]}(40 SC)|");
                Console.WriteLine($"LIN 3 | {nomesItensLoja[10]}(25 SC)| {nomesItensLoja[11]}(18 SC)| {nomesItensLoja[12]}(70 SC)| {nomesItensLoja[13]}(90 SC)| {nomesItensLoja[14]}(35 SC)|");
                Console.WriteLine($"LIN 4 | {nomesItensLoja[15]}(25 SC)| {nomesItensLoja[16]}(60 SC)| {nomesItensLoja[17]}(35 SC)| {nomesItensLoja[18]}(30 SC)| {nomesItensLoja[19]}(55 SC)|");
                Console.WriteLine($"LIN 5 | {nomesItensLoja[20]}(99 SC)| {nomesItensLoja[21]}(15 SC)| {nomesItensLoja[22]}(25 SC)| {nomesItensLoja[23]}(10 SC)| {nomesItensLoja[24]}(25 SC)|");
                Console.WriteLine("-------------------------------------------------------------------------");
                Console.WriteLine(" Digite 0 na linha para VOLTAR à Seleção de Níveis.");
                Console.WriteLine("=========================================================================");

                Console.Write("\nEscolha a LINHA (1 a 5) ou 0 para Sair: ");
                if (!int.TryParse(Console.ReadLine(), out int linha)) continue;
                if (linha == 0) break; 

                if (linha < 1 || linha > 5)
                {
                    Console.WriteLine("\n[ERRO] Linha inválida!");
                    Console.ReadKey();
                    continue;
                }  

                Console.Write("Escolha a COLUNA (1 a 5): ");
                if (!int.TryParse(Console.ReadLine(), out int coluna) || coluna < 1 || coluna > 5)
                {
                    Console.WriteLine("\n[ERRO] Coluna inválida!");
                    Console.ReadKey();
                    continue;
                }

                // Calcula o número do item correspondente (1 a 25)
                int numeroItem = ((linha - 1) * 5) + coluna;
                int custoItem = precosItensLoja[numeroItem - 1];
                string nomeDoItemEscolhido = nomesItensLoja[numeroItem - 1];

                if (scrapCoins >= custoItem)
                {
                    scrapCoins -= custoItem;
                    Console.WriteLine($"\n[SUCESSO] Você comprou: {nomeDoItemEscolhido} por {custoItem} ScrapCoins!");
                    Console.WriteLine($"\n==================================================");
                    Console.WriteLine($"   🛒 COMPRA REALIZADA COM SUCESSO!");
                    Console.WriteLine($"==================================================");
                    Console.WriteLine($" Item Adquirido: {nomeDoItemEscolhido}");
                    Console.WriteLine($" Preço do Item:  -{custoItem} ScrapCoins (Deduzidos)");
                    Console.WriteLine($" Saldo Atual:    {scrapCoins} ScrapCoins restantes");
                    Console.WriteLine($"==================================================");

                    switch (numeroItem)
                    {
                        case 1:  racao ++; Console.WriteLine("Comprou ração!"); inventory.Add("Ração"); break;
                        case 2:  garrafadeAgua ++;  Console.WriteLine("Comprou garrafa de água!"); inventory.Add("Garrafa de Água"); break;
                        case 3:  energetico ++;  Console.WriteLine("Comprou Energético!"); inventory.Add("Energético"); break;
                        case 4:  kitMedico ++;  Console.WriteLine("Comprou kit médico!"); inventory.Add("Kit Médico"); break;
                        case 5:  bandagem ++; Console.WriteLine("Comprou bandagem!"); inventory.Add("Bandagem"); break;
                        case 6:
                            if (colete)
                            {
                                Console.WriteLine("\n[LOJISTA] Você já tinha um desses... mas quem sou eu para recusar dinheiro?");
                                break;
                            }
                            colete = true;
                            defesaBonus += 10;
                            Console.WriteLine($"\n[ITEM] {nomeDoItemEscolhido} equipado! Defesa +10, POBRE!");
                            break;

                        case 7:
                            if (armaduradeCouro)
                            {
                                Console.WriteLine("\n[LOJISTA] Você já tinha um desses... mas quem sou eu para recusar dinheiro?");
                                break;
                            }
                            armaduradeCouro = true;
                            defesaBonus += 15;
                            Console.WriteLine($"\n[ITEM] {nomeDoItemEscolhido} equipado! Defesa +15, POBRE EM DOBRO KK!");
                            break;

                        case 8:
                            if (paladino)
                            {
                            armaduraMilitarReforcada = true;
                            defesaBonus += 50;
                            estaminaP -= 1;
                            Console.WriteLine("\n[EFEITO] Armadura Militar Reforçada equipada! Defesa +50, Estamina -1");
                            }
                            else
                            {
                            scrapCoins += custoItem;
                            Console.WriteLine("\n[BLOQUEADO] Apenas PALADINOS podem usar este item.");
                            Console.WriteLine("Seu dinheiro foi devolvido.");
                            }
                            break;
                        case 9:
                            if (armaduraMilitar)
                            {
                                Console.WriteLine("\n[LOJISTA] Você já tinha um desses... mas quem sou eu para recusar dinheiro?");
                                break;
                            }
                            armaduraMilitar = true;
                            defesaBonus += 25;
                            Console.WriteLine($"\n[ITEM] {nomeDoItemEscolhido} equipado! Defesa +25");
                            break;

                        case 10:
                            if (manopola)
                            {
                                Console.WriteLine("\n[LOJISTA] Você já tinha um desses... mas quem sou eu para recusar dinheiro?");
                                break;
                            }
                            manopola = true;
                            forcaBonus += 4;
                            Console.WriteLine($"\n[ITEM] {nomeDoItemEscolhido} equipado! Força +4");
                            break;

                        case 11:
                            if (socoIngles)
                            {
                                Console.WriteLine("\n[LOJISTA] Você já tinha um desses... mas quem sou eu para recusar dinheiro?");
                                break;
                            }
                            socoIngles = true;
                            forcaBonus += 5;
                            Console.WriteLine($"\n[ITEM] {nomeDoItemEscolhido} equipado! Força +5");
                            break;

                        case 12:
                            if (luvas)
                            {
                                Console.WriteLine("\n[LOJISTA] Você já tinha um desses... mas quem sou eu para recusar dinheiro?");
                                break;
                            }
                            luvas = true;
                            forcaBonus += 2;
                            Console.WriteLine($"\n[ITEM] {nomeDoItemEscolhido} equipado! Força +2,  POBRE!");
                            break;

                        case 13:
                            if (RifledePrecisão)
                            {
                                Console.WriteLine("\n[LOJISTA] Você já tinha um desses... mas quem sou eu para recusar dinheiro?");
                                break;
                            }
                            RifledePrecisão = true;
                            danoBonus += 25;
                            Console.WriteLine($"\n[ITEM] {nomeDoItemEscolhido} equipado! Dano +25, Na cabeça ou na mão?");
                            break;

                        case 14:
                            if (SniperAWM)
                            {
                                Console.WriteLine("\n[LOJISTA] Você já tinha um desses... mas quem sou eu para recusar dinheiro?");
                                break;
                            }
                            SniperAWM = true;
                            danoBonus += 50;
                            estaminaP -= 1;
                            Console.WriteLine($"\n[ITEM] {nomeDoItemEscolhido} equipado! Dano +50, Estamina -1");
                            break;

                        case 15:
                            if (eagle)
                            {
                                Console.WriteLine("\n[LOJISTA] Você já tinha um desses... mas quem sou eu para recusar dinheiro?");
                                break;
                            }
                            eagle = true;
                            danoBonus += 20;
                            Console.WriteLine($"\n[ITEM] {nomeDoItemEscolhido} equipado! Dano +20");
                            break;

                        case 16:
                            if (glock)
                            {
                                Console.WriteLine("\n[LOJISTA] Você já tinha um desses... mas quem sou eu para recusar dinheiro?");
                                break;
                            }
                            glock = true;
                            danoBonus += 10;
                            Console.WriteLine($"\n[ITEM] {nomeDoItemEscolhido} equipado! Dano +10, POBRE!");
                            break;

                        case 17:
                            if (marchadoPesado)
                            {
                                Console.WriteLine("\n[LOJISTA] Você já tinha um desses... mas quem sou eu para recusar dinheiro?");
                                break;
                            }
                            marchadoPesado = true;
                            forcaBonus += 20;
                            Console.WriteLine($"\n[ITEM] {nomeDoItemEscolhido} equipado! Força +20");
                            break;

                        case 18:
                            if (marchadoTatico)
                            {
                                Console.WriteLine("\n[LOJISTA] Você já tinha um desses... mas quem sou eu para recusar dinheiro?");
                                break;
                            }
                            marchadoTatico = true;
                            forcaBonus += 8;
                            Console.WriteLine($"\n[ITEM] {nomeDoItemEscolhido} equipado! Força +8");
                            break;

                        case 19:
                            if (cutelo)
                            {
                                Console.WriteLine("\n[LOJISTA] Você já tinha um desses... mas quem sou eu para recusar dinheiro?");
                                break;
                            }
                            cutelo = true;
                            forcaBonus += 7;
                            Console.WriteLine($"\n[ITEM] {nomeDoItemEscolhido} equipado! Força +7, POBRE!");
                            break;

                        case 20:
                            if (m4A1)
                            {
                                Console.WriteLine("\n[LOJISTA] Você já tinha um desses... mas quem sou eu para recusar dinheiro?");
                                break;
                            }
                            m4A1 = true;
                            danoBonus += 35;
                            estaminaP -= 1;
                            Console.WriteLine($"\n[ITEM] {nomeDoItemEscolhido} equipado! Dano +35, Estamina -1");
                            break;

                        case 21:
                            if (miniGun)
                            {
                                Console.WriteLine("\n[LOJISTA] Você já tinha um desses... mas quem sou eu para recusar dinheiro?");
                                break;
                            }
                            miniGun = true;
                            danoBonus += 60;
                            estaminaP -= 1;
                            Console.WriteLine($"\n[ITEM] {nomeDoItemEscolhido} equipado! Dano +60, Estamina -1");
                            break;

                        case 22:
                            maconha = true;
                            Console.WriteLine($"\n[EFEITO] Você consumiu {nomeDoItemEscolhido}. Relaxamento estranho...");
                            break;

                        case 23:
                            if (panodePrato)
                            {
                                Console.WriteLine("\n[LOJISTA] Você já tinha um desses... mas quem sou eu para recusar dinheiro?");
                                break;
                            }
                            panodePrato = true;
                            estaminaP += 1;
                            Console.WriteLine($"\n[EFEITO] {nomeDoItemEscolhido} limpa o suor. Estamina +1, Fedorento!");
                            break;

                        case 24:
                            if (roupaRasgada)
                            {
                                Console.WriteLine("\n[LOJISTA] Você já tinha um desses... mas quem sou eu para recusar dinheiro?");
                                break;
                            }
                            roupaRasgada = true;
                            defesaBonus -= 1;
                            Console.WriteLine($"\n[EFEITO] {nomeDoItemEscolhido} não protege nada. Defesa -1");
                            break;
                        case 25:
                            Console.WriteLine("\n[GACHA] Você ativou a Caixa Surpresa! Girando a roleta da sorte...");
        
                            // Cria o gerador de números aleatórios

                            int indiceSorteado = random.Next(0, premiosGacha.Length);
                            string premioGanho = premiosGacha[indiceSorteado];
                            Console.WriteLine($"\n==================================");
                            Console.WriteLine($"🎉 PARABÉNS! Você ganhou: {premioGanho.ToUpper()}!");
                            Console.WriteLine($"==================================");
                            switch (premioGanho)
                            {
                                case "Maconha":
                                    Console.WriteLine("[EFEITO] Você ficou relaxado. Fome +5 e Vida -3!");
                                    fome += 5;
                                    vidaP -= 3;
                                    break;

                                case "Pano de Prato":
                                    Console.WriteLine("[EFEITO] Inútil no combate, mas limpa o suor. Estamina +1!");
                                    estaminaP += 1;
                                    break;

                                case "Roupa Rasgada":
                                    Console.WriteLine("[EFEITO] Não te protege nada. Defesa -1!");
                                    defesaBonus -= 1;
                                    break;

                                case "Pneu Furado":
                                    Console.WriteLine("[EFEITO] Peso morto... Você tropeçou nele. Vida -3!");
                                    vidaP -= 3;
                                    break;

                                case "MiniGun":
                                    Console.WriteLine("[EFEITO] RECOMPENSA LENDÁRIA! Força aumentada drasticamente. dano + 30, estamina -1!");
                                    miniGun = true;
                                    danoBonus += 60;
                                    estaminaP -= 1;
                                    break;

                                case "Armadura Militar Reforçada":
                                    if (paladino && !armaduraMilitarReforcada)
                                    {
                                        Console.WriteLine("[EFEITO] RECOMPENSA LENDÁRIA! Defesa máxima atingida. Defesa +50!");
                                        armaduraMilitarReforcada = true;
                                        defesaBonus += 50;
                                        estaminaP -= 1;
                                    }
                                    else
                                    {
                                        // Penalidade por não ser da classe correta.
                                        Console.WriteLine("\n[MALDIÇÃO] A Armadura Militar Reforçada rejeitou o seu corpo!");
                                        Console.WriteLine("Você não tem a classe Paladino/Ou é muito ganancioso. Em vez dela, você tropeçou em um Pneu Furado! Vida -5.");
                                        vidaP -= 5;
                                    }
                                    break;
                            }  
                            break;
                    }
                    UpdateGame();
                }
                else
                {
                    Console.WriteLine($"\n[ERRO] ScrapCoins insuficientes! {nomeDoItemEscolhido} custa {custoItem} SC.");
                }

                Console.WriteLine("\nPressione qualquer tecla para atualizar a loja...");
                Console.ReadKey();
            }
        }
        static void ExibirMensagemClasseAoEntrar(int nivel)
        {
            Console.Clear();
            Console.WriteLine($"================ NÍVEL {nivel} ================");
        
            if (andarilho)
            {
                Console.WriteLine("Como um Andarilho, seus pés calejados conhecem as estradas.");
                Console.WriteLine("Você ajusta sua capa e avança nas sombras do labirinto...");
            }
            else if (mercenario)
            {
                Console.WriteLine("O cheiro de moedas te guia. Como um Mercenário, este é só mais um contrato.");
                Console.WriteLine("Você puxa sua lâmina e avança com um sorriso frio...");
            }
            else if (paladino)
            {
                Console.WriteLine("A luz da justiça brilha em sua armadura pesada. Como um Paladino, você não teme o mal.");
                Console.WriteLine("Você ergue seu escudo sagrado e marcha em frente...");
            }
            else if (vigilante)
            {
                Console.WriteLine("Seus olhos atentos captam cada ruído. Como um Vigilante, a desordem acaba hoje.");
                Console.WriteLine("Você ajusta seus equipamentos e avança silenciosamente...");
            }

            Console.WriteLine("=======================================");
            Console.WriteLine("\nPressione qualquer tecla para começar...");
            Console.ReadKey();
        }

        //=================================================
        // HUD
        //=================================================

        static void ShowHUD()
        {
            while(true)
            {
                Console.Clear();

                string? personagem = playerName;
                string raca = playerRace;
                string profissao = playerClass;
                
                int hp = vidaP;
                int hpmax = vidaP;

                int hunger = fome;
                int hungerMax = FOME_MAXIMA;

                int thirst = sede;
                int thirstMax = SEDE_MAXIMA;

                int deffense = defesaP;

                Console.Clear();
                Console.WriteLine("====================================================================================================");
                Console.WriteLine($"| Personagem: " + playerName + " |");
                Console.WriteLine($"| Raça: " + playerRace + " |");
                Console.WriteLine($"| Profissão: " + playerClass + " |");
                Console.WriteLine("|--------------------------------------------------------------------------------------------------|");
                Console.WriteLine($"| Vida: " + vidaP + " |");
                Console.WriteLine($"| Defesa: " + defesaP + " |");
                Console.WriteLine($"| Dano: " + dano + " |");
                Console.WriteLine($"| Força: " + forca + " |");
                Console.WriteLine($"| Estamina: " + estaminaP + " |");
                Console.WriteLine("|--------------------------------------------------------------------------------------------------|");
                Console.WriteLine($"| Fome: " + fome + " |");
                Console.WriteLine($"| Sede: " + sede + " |");
                Console.WriteLine("|--------------------------------------------------------------------------------------------------|");
                Console.WriteLine($"| Vida: " + (hp, hpmax),-40  + " |");
                Console.WriteLine($"| Fome: " + (hunger, hungerMax),-40 + " |");
                Console.WriteLine($"| Sede: " + (thirst, thirstMax),-40 + " |");
                Console.WriteLine("|                                                                                                  |");
                Console.WriteLine("====================================================================================================");
                Console.ReadLine();
                break;
            }
        }

        //=================================================
        // MAPA
        //=================================================

        static void ShowCurrentRoom()
        {
            Console.Clear();

            switch(currentRoom)
            {
                case 0:
                    Console.WriteLine("================================");
                    Console.WriteLine("ENTRADA DO BUNKER");
                    Console.WriteLine("================================");
                    Console.WriteLine("Você encontra três corredores.");
                    Console.WriteLine();
                    Console.WriteLine("[1] Caminho A");
                    Console.WriteLine("[2] Caminho B");
                    Console.WriteLine("[3] Caminho C");
                    Console.WriteLine("[4] Mochila");
                    Console.WriteLine("[0] Voltar ao Menu de Níveis");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            currentRoom = 1;
                            break;

                        case "2":
                            currentRoom = 2;
                            break;

                        case "3":
                            currentRoom = 3;
                            break;

                        case "4":
                            OpenInventory();
                            break;

                        case "0":
                            gameRunning = false;
                            break;
                    }
                    break;

                case 1:
                    Console.WriteLine("========== SALA A ==========");
                    Console.WriteLine("Um corredor parcialmente destruído.");
                    Console.WriteLine();

                    Console.WriteLine("[1] Sala D");
                    Console.WriteLine("[2] Sala E");
                    Console.WriteLine("[3] Mochila");
                    Console.WriteLine("[0] Voltar para Entrada");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            currentRoom = 4;
                            break;

                        case "2":
                            currentRoom = 5;
                            break;

                        case "3":
                            OpenInventory();
                            break;

                        case "0":
                            currentRoom = 0;
                            break;
                    }
                    break;

                case 2:
                    Console.WriteLine("========== SALA B ==========");
                    Console.WriteLine("O corredor possui várias portas metálicas.");
                    Console.WriteLine();

                    Console.WriteLine("[1] Sala F");
                    Console.WriteLine("[2] Sala G");
                    Console.WriteLine("[3] Mochila");
                    Console.WriteLine("[0] Voltar para Entrada");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            currentRoom = 6;
                            break;

                        case "2":
                            currentRoom = 7;
                            break;

                        case "3":
                            OpenInventory();
                            break;

                        case "0":
                            currentRoom = 0;
                            break;
                    }
                    break;
                case 3:
                    Console.WriteLine("========== SALA C ==========");
                    Console.WriteLine("Você encontra sinais de sobreviventes.");
                    Console.WriteLine();

                    Console.WriteLine("[1] Sala H");
                    Console.WriteLine("[2] Sala I");
                    Console.WriteLine("[3] Mochila");
                    Console.WriteLine("[0] Voltar para Entrada");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            currentRoom = 8;
                            break;

                        case "2":
                            currentRoom = 9;
                            break;

                        case "3":
                            OpenInventory();
                            break;

                        case "0":
                            currentRoom = 0;
                            break;
                    }
                    break;

                case 4:
                    Console.WriteLine("========== SALA D ==========");
                    Console.WriteLine("Cabos elétricos energizados explodem.");
                    Console.WriteLine("Você pisa numa armadilha.");

                    vidaP -= 20;

                    Console.WriteLine("Vida -20, Besta KK.");
                    Console.WriteLine();

                    Console.WriteLine("[1] Voltar para Sala A");
                    Console.WriteLine("[2] Mochila");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            currentRoom = 1;
                            break;

                        case "2":
                            OpenInventory();
                            break;
                    }
                    break;

                case 5:
                    Console.WriteLine("========== SALA E ==========");

                    Console.WriteLine("Há vários computadores antigos.");

                    if (paladino)
                    {
                        if (!achouArquivos)
                        {
                            Console.WriteLine("Você encontrou arquivos militares secretos.");

                            achouArquivos = true;
                            UpdateGame();

                            Console.WriteLine("Objetivo do Paladino concluído.");
                        }
                        else
                        {
                            Console.WriteLine("Nada mais resta aqui.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Computadores antigos sem utilidade. Pega 1 moedinha, estou com pena de você. +1 ScrapCoin");
                        scrapCoins++;
                    }

                    Console.WriteLine();
                    Console.WriteLine("[1] Voltar para Sala A");
                    Console.WriteLine("[2] Mochila");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            currentRoom = 1;
                            break;

                        case "2":
                            OpenInventory();
                            break;
                    }
                    break;
                case 6:

                    Console.WriteLine("========== SALA F ==========");

                    if (andarilho || mercenario)
                    {
                        if (!achouProjetoPurificador)
                        {
                            Console.WriteLine("Você encontrou o Projeto do Purificador.");

                            achouProjetoPurificador = true;
                            UpdateGame();

                            Console.WriteLine("Objetivo concluído.");
                        }
                        else
                        {
                            Console.WriteLine("O laboratório está vazio.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Laboratório destruído.");
                    }

                    Console.WriteLine();
                    Console.WriteLine("[1] Voltar para Sala B");
                    Console.WriteLine("[2] Abrir Mochila");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            currentRoom = 2;
                            break;

                        case "2":
                            OpenInventory();
                            break;
                    }
                    break;

                case 7:

                    Console.WriteLine("========== SALA G ==========");
                    Console.WriteLine("O piso desaba.");

                    vidaP -= 15;

                    Console.WriteLine("Vida -15, escorregou ai?");
                    Console.WriteLine();

                    Console.WriteLine("[1] Voltar para Sala B");
                    Console.WriteLine("[2] Abrir Mochila");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            currentRoom = 2;
                            break;

                        case "2":
                            OpenInventory();
                            break;
                    }
                    break;

                case 8:

                    Console.WriteLine("========== SALA H ==========");

                    if (vigilante)
                    {
                        if (!ajudouMercador)
                        {
                            Console.WriteLine("Você protegeu o mercador de saqueadores.");

                            ajudouMercador = true;
                            UpdateGame();

                            Console.WriteLine("Objetivo concluído.");
                        }
                        else
                        {
                            Console.WriteLine("O mercador agradece novamente.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("O mercador não confia em você.");
                    }

                    Console.WriteLine();
                    Console.WriteLine("[1] Voltar para Sala C");
                    Console.WriteLine("[2] Abrir Mochila");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            currentRoom = 3;
                            break;

                        case "2":
                            OpenInventory();
                            break;
                    }
                    break;

                case 9:

                    Console.WriteLine("========== SALA I ==========");

                    if (!achouChave)
                    {
                        Console.WriteLine("Você encontrou a chave do bunker.");

                        achouChave = true;
                        UpdateGame();

                        Console.WriteLine("Agora você pode acessar o próximo nível.");
                    }

                    Console.WriteLine();
                    Console.WriteLine("Ao sair, uma armadilha dispara.");

                    vidaP -= 10;

                    Console.WriteLine();
                    Console.WriteLine("[1] Voltar para Sala C");
                    Console.WriteLine("[2] Abrir Mochila");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            currentRoom = 3;
                            break;

                        case "2":
                            OpenInventory();
                            break;
                    }
                    break;
                case 10:

                    Console.Clear();

                    Console.WriteLine("================================");
                    Console.WriteLine("        NÍVEL 2");
                    Console.WriteLine("================================");
                    Console.WriteLine();
                    Console.WriteLine("Ao atravessar a enorme porta de aço...");
                    Console.WriteLine("Uma criatura mutante desperta.");
                    Console.WriteLine();
                    Console.WriteLine("MINI BOSS");
                    Console.WriteLine($"Vida: {vidaMiniBoss}/300");
                    Console.WriteLine();

                    if (!miniBossDerrotado)
                    {
                        CombateMiniBoss();
                    }
                    else
                    {
                        Console.WriteLine("O corpo do mini boss permanece caído.");
                        Console.WriteLine("Você venceu.");
                        nivel3Desbloqueado = true;
                        Console.WriteLine();

                        Console.WriteLine("[0] Voltar ao Menu de Níveis");

                        switch (Console.ReadLine())
                        {
                            case "0":
                                gameRunning = false;
                                break;
                        }
                    }

                    break;
            }
        }

        //=================================================
        // AÇÕES DO JOGADOR
        //=================================================

        static void PlayerAction()
        {
            Console.WriteLine("\n=============================================");
            Console.WriteLine(" [5] Dar um chute em uma porta pra espionar");
            Console.WriteLine("=============================================");
            Console.WriteLine($"        Fome: {fome} | Sede: {sede}");
            Console.Write("O que deseja fazer? ");

            string acao = Console.ReadLine();

            switch (acao)
            {
                case "5":

                    Console.WriteLine("\nVocê decide revirar os detritos do local em busca de sobrevivência...");

                    int sorteio = random.Next(1, 5);

                    if (sorteio == 1)
                    {
                        scrapCoins += 15;
                        Console.WriteLine("SUCESSO! Você encontrou um punhado de componentes! (+15 ScrapCoins)");
                    }
                    else if (sorteio == 2)
                    {
                        racao++;
                        Console.WriteLine("SUCESSO! Você achou uma embalagem de Ração militar no chão! (Ração +1)");
                    }
                    else if (sorteio == 3)
                    {
                        vidaP -= 20;
                        Console.WriteLine("Tá abusando, toma uma lapada na cara para ficar atento! (-20 de vida)");
                    }
                    else if (sorteio == 4)
                    {
                        estaminaP += 2;
                        vidaP--;
                        Console.WriteLine("Você achou um mendingo comendo uma sopa de cogumelos e caiu na porrada com ele.");
                        Console.WriteLine("Cê apanhou pra caramba do mendingo bombado, mas conseguiu beber a sopa.");
                        Console.WriteLine("===================== Estamina +2, vida - 1! ==========================");
                    }
                    else
                    {
                        Console.WriteLine("Você revira tudo, mas só encontra poeira e lixo inútil.");
                    }

                    Console.ReadKey();
                    break;

                    default:
                    break;
            }
        }
        //=================================================
        // COMBATE
        //=================================================
        static void CombateMiniBoss()
        {
            int ataqueInimigo = danoMiniBoss;

            while (vidaMiniBoss > 0 && vidaP > 0)
            {
                Console.Clear();

                Console.WriteLine("========== MINI BOSS ==========");
                Console.WriteLine($"Vida do Mini Boss: {vidaMiniBoss}/300");
                Console.WriteLine();

                Console.WriteLine($"Seu dano total: {DanoTotal()}");
                Console.WriteLine($"Sua defesa total: {DefesaTotal()}");
                Console.WriteLine($"Sua estamina total: {estaminaP}");
                Console.WriteLine();

                Console.WriteLine("[1] Atacar");
                Console.WriteLine("[2] Mochila");
                Console.WriteLine("[0] Fugir");

                string escolha = Console.ReadLine();

                switch (escolha)
                {
                    case "1":
                        if (estaminaP <= 0)
                            {
                                Console.WriteLine("\nVocê tentou levantar sua arma, mas estava completamente exausto...");
                                Console.WriteLine("O Mini Boss percebeu sua fraqueza, ele espirrou em você e você morreu, troxa kk.");
                                Console.WriteLine("Você estava cansado demais para reagir.");
                                vidaP = 0;
                                Console.ReadKey();
                                return;
                            }
                        estaminaP--;
                        vidaMiniBoss -= DanoTotal();
                        Console.WriteLine($"\nVocê causou {DanoTotal()} de dano!");
                        if (vidaMiniBoss < 0)
                            vidaMiniBoss = 0;
                        if (vidaMiniBoss == 0)
                        {
                            miniBossDerrotado = true;
                            Console.WriteLine("\nMini Boss derrotado!");
                            Console.ReadKey();
                            return;
                        }
                        Console.WriteLine("\nO Mini Boss contra-ataca!");

                        Random rng = new Random();
                        int ataqueBoss = rng.Next(1, 4); // 1, 2 ou 3

                        switch (ataqueBoss)
                        {
                            case 1:
                                Console.WriteLine("\nO Mini Boss desfere um golpe brutal!");
                                ReceberDano(ataqueInimigo);
                                break;
                            case 2:
                                Console.WriteLine("\nO Mini Boss usa SUPER SUGADA MALEVOLENTE!");
                                fome -= 50;
                                sede -= 50;
                                vidaP -= 20;
                                if (fome < 0) fome = 0;
                                if (sede < 0) sede = 0;
                                if (vidaP < 0) vidaP = 0;
                                Console.WriteLine("Você perdeu 50 de fome, 50 de sede e 20 de vida!");
                                break;
                            case 3:
                                Console.WriteLine("\nO Mini Boss toma uma OVERDOSE!");
                                vidaMiniBoss += 20;
                                if (vidaMiniBoss > 300)
                                    vidaMiniBoss = 300;
                                Console.WriteLine("O Mini Boss recuperou 20 de vida!");
                                break;
                        }
                        Console.ReadKey();
                        break;
                    case "2":
                        OpenInventory();
                        break;
                    case "0":
                        Console.WriteLine("\nVocê fugiu da batalha.");
                        gameRunning = false;
                        Console.ReadKey();
                        return;
                    default:
                        Console.WriteLine("Opção inválida.");
                        Console.ReadKey();
                        break;
                }
            }
        } 
        static void ReceberDano(int danoInimigo)
        {
            int danoRecebido = danoInimigo - DefesaTotal();

            danoRecebido = Math.Max(0, danoRecebido);

            vidaP -= danoRecebido;

            Console.WriteLine($"\nVocê recebeu {danoRecebido} de dano!");
        }
        //=================================================
        // INVENTÁRIO
        //=================================================

        static void OpenInventory()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==================================================");
                Console.WriteLine("                 MOCHILA DE SUCATA              ");
                Console.WriteLine("==================================================");
                if (inventory.Count == 0)
                {
                    Console.WriteLine("Mochila vazia!");
                }
                else
                {
                    for (int i = 0; i < inventory.Count; i++)
                    {
                        Console.WriteLine($" [{i + 1}] {inventory[i]}");
                    }
                }
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine(" [0] Voltar ao Jogo");
                Console.WriteLine("==================================================");
                Console.Write("\nEscolha o número do item que deseja CONSUMIR: ");

                if (!int.TryParse(Console.ReadLine(), out int escolha)) continue;
                if (escolha == 0) break;

                if (escolha >= 1 && escolha <= inventory.Count)
                {
                    // Envia a escolha do jogador para processar o uso do item
                    UseItem(escolha); 
                }
                else
                {
                    Console.WriteLine("\nOpção inválida!");
                    Console.ReadKey();
                }
            }
        }
        static void UseItem(int opcao)
        {
            
            Console.Clear();
            string item = inventory[opcao - 1];
            switch (item)
            {
                case "Ração": // Ração
                    if (racao > 0)
                    {
                        racao--;    
                        fome += 25;
                        Console.WriteLine("Você comeu uma Ração saborosa! Fome +25.");
                    }
                    else Console.WriteLine("Você não tem nenhuma Ração na mochila.");
                    break;
                case "Garrafa de Água":
                    if (garrafadeAgua > 0)
                    {
                        garrafadeAgua--;
                        sede += 30;
                        Console.WriteLine("Você bebeu uma Garrafa de Água cristalina! Sede +30.");
                    }
                    else Console.WriteLine("Você não tem nenhuma Garrafa de Água na mochila.");
                    break;
                case "Energético": // Energético
                    if (energetico > 0)
                    {
                        energetico--;
                        estaminaP += 8;
                        Console.WriteLine("Você virou um Energético! Estamina +8.");
                    }
                    else Console.WriteLine("Você não tem nenhum Energético na mochila.");
                    break;
                case "Kit Médico":
                    if (kitMedico > 0)
                    {
                        kitMedico--;
                        vidaP += 50;
                        Console.WriteLine("Você usou o Kit Médico! Vida +50.");
                    }
                    else Console.WriteLine("Você não tem nenhum Kit Médico na mochila.");
                    break;
                case "Bandagem":
                    if (bandagem > 0)
                    {
                        bandagem--;
                        vidaP += 15;
                        Console.WriteLine("Você aplicou uma Bandagem! Vida +15.");
                    }
                    else Console.WriteLine("Você não tem nenhuma Bandagem na mochila.");
                    break;
                    default:
                    Console.WriteLine("Opção inválida de item.");
                    break;
            }
            inventory.RemoveAt(opcao - 1);
            UpdateGame(); 
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
        //=================================================
        // ATUALIZAÇÃO DO JOGO
        //=================================================
        static void UpdateGame()
        {
            if (DateTime.Now >= proximaReducaoStatus)
            {
                fome -= 3; // Perde de fome e sede a cada 10 segundos
                sede -= 5; 
                // Reagenda a próxima perda para daqui a mais 10 segundos
                proximaReducaoStatus = DateTime.Now.AddSeconds(10);
            }
            // --- VALIDAÇÃO EXCLUSIVA DO NÍVEL 1 ---
            if (!nivel2Desbloqueado && achouChave)
            {
                if (paladino && achouArquivos)
                {
                    nivel2Desbloqueado = true;
                    Console.WriteLine("\nNÍVEL 2 DESBLOQUEADO!");
                    Console.ReadKey();
                }
                else if ((andarilho || mercenario) && achouProjetoPurificador)
                {
                    nivel2Desbloqueado = true;
                    Console.WriteLine("\nNÍVEL 2 DESBLOQUEADO!");
                    Console.ReadKey();
                }
                else if (vigilante && ajudouMercador)
                {
                    nivel2Desbloqueado = true;
                    Console.WriteLine("\nNÍVEL 2 DESBLOQUEADO!");
                    Console.ReadKey();
                }
            }
            vidaP = Math.Clamp(vidaP, 0, VIDA_MAXIMA);
            fome = Math.Clamp(fome, 0, FOME_MAXIMA);
            estaminaP = Math.Clamp(estaminaP, 0, ESTAMINA_MAXIMA);
            sede = Math.Clamp(sede, 0, SEDE_MAXIMA);
        }
        //=================================================
        // FINALIZAÇÃO
        //=================================================
        static bool CheckGameOver()
        {
        // Se qualquer um dos status vitais zerar, o jogo termina
            if (vidaP <= 0 || fome <= 0 || sede <= 0)
            {
                return true;
            }
            return false;
        }
        static void ShowEnding()
        {
            if (vidaP <= 0 || fome <= 0 || sede <= 0)
            { 
                Console.Clear();
                Console.WriteLine("==================================");
                Console.WriteLine("          FIM DA JORNADA          ");
                Console.WriteLine("==================================");
                Console.WriteLine("\n[ VOCÊ MORREU ]");
                if (vidaP <= 0) Console.WriteLine("Seus pontos de vida chegaram a 0. Aprende a jogar!");
                else if (fome <= 0) Console.WriteLine("Você morreu de fome extrema. Até mendingo sabe procurar o que comer e você não!");
                else if (sede <= 0) Console.WriteLine("Você morreu de desidratação. Quer um copo de água aí?");
            
                Console.WriteLine("===========================================================");
                Console.WriteLine("\nPressione qualquer tecla para voltar ao menu principal...");
                Console.ReadKey();
                return;
            }

             if (nivel2Desbloqueado && 
                nivel3Desbloqueado && 
                nivel4Desbloqueado && 
                nivel5Desbloqueado && 
                completeGame && 
                bossMorto && 
                salvarPessoasdoBunker)
            {
                Console.Clear();
                Console.WriteLine("=====================================");
                Console.WriteLine("VOCÊ COMPLETOU O JOGO COMO ANDARILHO!");
                Console.WriteLine("=====================================");
                Console.WriteLine("\n[    Você salvou as pessoas do Refúgio e derrotou o Boss,]");
                Console.WriteLine("\n[ impedindo a invasão do Forte Militar Corrupto, mas agora só ]");
                Console.WriteLine("\n[    resta continuar sua jornada. Pelo menos até quando os ]");
                Console.WriteLine("\n[ suprimentos acabarem -- se não as próprias pessoas se matarem ]");
                Console.WriteLine("\n[              antes --, seguindo a natureza humana. ]");
                Console.WriteLine("     ========================================================");
                Console.WriteLine("\n   Pressione qualquer tecla para voltar ao menu de níveis...");
                Console.ReadKey();
                return;
            }
            if (nivel2Desbloqueado &&
                nivel5Desbloqueado &&
                achouChave &&
                completeGame &&
                bossMorto &&
                escapar)
            {
                Console.Clear();
                Console.WriteLine("=====================================");
                Console.WriteLine("VOCÊ COMPLETOU O JOGO COMO MERCENÁRIO!");
                Console.WriteLine("=====================================");

                Console.WriteLine("\nApós fugir do Forte levando equipamentos roubados,");
                Console.WriteLine("você encontrou uma cidade isolada e investiu tudo");
                Console.WriteLine("o que conquistou durante suas missões.");
                Console.WriteLine("\nCom o tempo tornou-se líder daquele povo,");
                Console.WriteLine("fortificando a cidade e acumulando riquezas.");
                Console.WriteLine("\nSeu maior investimento foi um cassino.");
                Console.WriteLine("Agora você lucra sem precisar arriscar a própria vida.");
                Console.WriteLine("\nA casa sempre ganha.");

                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
                return;
            }
            if (completeGame &&
                nivel5Desbloqueado &&
                bossMorto &&
                completarVinganca)
            {
                Console.Clear();
                Console.WriteLine("=====================================");
                Console.WriteLine("VOCÊ COMPLETOU O JOGO COMO PALADINO!");
                Console.WriteLine("=====================================");

                Console.WriteLine("\nApós derrotar seu antigo companheiro Kraly,");
                Console.WriteLine("ele revela a corrupção dentro do Forte.");
                Console.WriteLine("\nAntes de morrer, provoca uma explosão");
                Console.WriteLine("que consome toda a instalação em chamas.");
                Console.WriteLine("\nDepois de entregar os arquivos finais,");
                Console.WriteLine("você abandona os militares corruptos");
                Console.WriteLine("e cria seu próprio esquadrão de elite.");
                Console.WriteLine("\nSua guerra ainda não terminou.");

                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
                return;
            }    
            if (bossMorto &&
                salvarPessoasdoBunker &&
                completeGame &&
                vigilante)
            {
                Console.Clear();
                Console.WriteLine("=====================================");
                Console.WriteLine("VOCÊ COMPLETOU O JOGO COMO VIGILANTE!");
                Console.WriteLine("=====================================");

                Console.WriteLine("\nApós destruir o Forte Militar Corrupto,");
                Console.WriteLine("restaram apenas fogo, fumaça e silêncio.");
                Console.WriteLine("\nEm um galpão escondido, você encontra");
                Console.WriteLine("um filhote da mesma espécie dos Lagartos.");
                Console.WriteLine("Seus pais estavam mortos.");
                Console.WriteLine("\nVocê o resgata, lembrando-se de quando");
                Console.WriteLine("alguém salvou sua própria vida.");
                Console.WriteLine("\nDessa vez, você conseguiu retribuir o favor.");

                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
                return;
            }
            if (bossMorto && !achouArquivos && !achouProjetoPurificador && !ajudouMercador && !escapar && !completarVinganca)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("════════════════════════════════════════════════════════════");
                Console.WriteLine("                      FINAL NEUTRO");
                Console.WriteLine("════════════════════════════════════════════════════════════");
                Console.ResetColor();

                Console.WriteLine("\nLogo após derrotar o Boss e escapar do Forte, você finalmente conquistou sua liberdade. " +
                                "Sem se envolver com os problemas dos sobreviventes ou buscar respostas para os mistérios " +
                                "escondidos naquele lugar, seguiu seu próprio caminho. " +
                                "O Forte permaneceu como uma cicatriz esquecida do velho mundo, enquanto os sobreviventes " +
                                "continuaram enfrentando suas próprias batalhas. Você viveu para contar sua história, " +
                                "mas sua passagem por aquele lugar mudou apenas o seu próprio destino.");

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\nFinal Neutro Obtido!");
                Console.ResetColor();
            }
        }
        static void CloseGame()
        {
            Console.Clear();
            Console.WriteLine("Obrigado por jogar!");
        }
        //=================================================
        // MENUS AUXILIARES
        //=================================================
        static void Credits()
        {
            Console.WriteLine($"                                                          ");
            Console.WriteLine($"======================== CRÉDITOS ========================");
            Console.WriteLine($"                                                          ");
            Console.WriteLine($"======================== Equipe: =========================");
            Console.WriteLine($"======================== NOVA GEN ========================");
            Console.WriteLine($"                                                          ");
            Console.WriteLine($"Ideia principal: Yuri");
            Console.WriteLine($"Programação: Gabriel (programador principal) e Bryan (programador auxiliar)");
            Console.WriteLine($"Documentação do projeto: Bryan");
            Console.WriteLine($"Inspirações: Fallout 1 (1997) e Fallout 2 (1998)");
            Console.WriteLine($"Instituição: Senai (Jacarecanga)");
            Console.WriteLine($"                                                          ");
            Console.WriteLine($"                                                          ");
            Console.WriteLine($"É os guri, vambora!!!");
            Console.ReadKey();
        }
    }