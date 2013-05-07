using System.Collections.Generic;

namespace Fishing
{
    internal class Dictionaries
    {
        #region Constructor
        private Dictionaries()
        {
        }
        #endregion //Constructor

        #region FishDictionary
        public static Dictionary<string, int> fishDictionary = new Dictionary<string, int>()
        {
            // ITEMS & QUEST & 0 SKILL
            {"1 Gil", 0},
            {"100 Gil", 0},
            {"Arrowwood Log", 688},
            {"Bugbear Mask", 1624},
            {"Copper Ring", 13454},
            {"Coral Fragment", 887},
            {"Damp Scroll", 1210},
            {"Fish Scale Shield", 12316},
            {"Lamp Marimo", 2216},
            {"Moblin Mask", 1638},
            {"Mythril Dagger", 16451},
            {"Mythril Sword", 16537},
            {"Norg Shell", 1135},
            {"Pamtam Kelp", 624},
            {"Ripped Cap", 591},
            {"Rusty Bucket", 90},
            {"Rusty Cap", 12522},
            {"Rusty Greatsword", 16606},
            {"Rusty Leggings", 14117},
            {"Rusty Pick", 16655},
            {"Rusty Subligar", 14242},
            {"Silver Ring", 13456},
            // FISH
            // A
            {"Abaia", 5476},
            {"Ahtapot", 5455},
            {"Alabaligi", 5461},
            {"Armored Picses", 4316},
            {"Aurora Bass", 5818},
            // B
            {"Barnacle", 5954},
            {"Bastore Bream", 4461},
            {"Bastore Sardine", 4360},
            {"Bastore Sardine x2", 4360},
            {"Bastore Sardine x3", 4360},
            {"Bastore Sweeper", 5473},
            {"Betta", 5139},
            {"Bhefhel Marlin", 4479},
            {"Bibiki Slug", 5122},
            {"Bibiki Urchin", 4318},
            {"Bibikibo", 4314},
            {"Black Bubble-Eye", 4311},
            {"Black Eel", 4429},
            {"Black Ghost", 5138},
            {"Black Sole", 4384},
            {"Bladefish", 4471},
            {"Blindfish", 4313},
            {"Blowfish", 5812},
            {"Bluetail", 4399},
            {"Brass Loach", 5469},
            // C
            {"Ca Cuong", 5474},
            {"Caedarva Frog", 5465},
            {"Calico Comet", 5715},
            {"Cave Cherax", 4309},
            {"Cheval Salmon", 4379},
            {"Cobalt Jellyfish", 4443},
            {"Cone Calamary", 5128},
            {"Cone Calamary x2", 5128},
            {"Cone Calamary x3", 5128},
            {"Copper Frog", 4515},
            {"Coral Butterfly", 4580},
            {"Crayfish", 4472},
            {"Crescent Fish", 4473},
            {"Crocodilos", 5814},
            {"Crystal Bass", 4528},
            // D
            {"Dark Bass", 4428},
            {"Denizanasi", 5447},
            {"Dil", 5457},
            {"Dorado Gar", 5813},
            // E
            {"Elshimo Frog", 4290},
            {"Elshimo Newt", 4579},
            {"Emperor Fish", 4454},
            // F
            {"Fat Greedie", 4501},
            {"Forest Carp", 4289},
            // G
            {"Garpike", 5472},
            {"Gavial Fish", 4477},
            {"Gerrothorax", 5471},
            {"Giant Catfish", 4469},
            {"Giant Chirai", 4308},
            {"Giant Donko", 4306},
            {"Gigant Octopus", 5475},
            {"Gigant Squid", 4474},
            {"Gold Carp", 4427},
            {"Gold Lobster", 4383},
            {"Greedie", 4500},
            {"Grimmonite", 4304},
            {"Gugru Tuna", 4480},
            {"Gugrusaurus", 5127},
            {"Gurnard", 5132},
            // H
            {"Hakuryu", 5539},
            {"Hamsi", 5449},
            {"Hamsi x2", 5449},
            {"Hamsi x3", 5449},
            // I
            {"Icefish", 4470},
            {"Icefish x2", 4470},
            {"Icefish x3", 4470},
            {"Istakoz", 5453},
            {"Istavrit", 5136},
            {"Istiridye", 5456},
            // J
            {"Jacknife", 5123},
            {"Jungle Catfish", 4307},
            // K
            {"Kalamar", 5448},
            {"Kalamar x2", 5448},
            {"Kalamar x3", 5448},
            {"Kalkanbaligi", 5140},
            {"Kaplumbaga", 5464},
            {"Kayabaligi", 5460},
            {"Kilicbaligi", 5451},
            {"King Perch", 5816},
            {"Kokuryu", 5540},
            // L
            {"Lakerda", 5450},
            // Lamp Merimo in items
            // Land Crab Meat can't be fished up
            {"Lik", 5129},
            {"Lionhead", 4312},
            {"Lungfish", 4315},
            // M
            {"Matsya", 5468},
            {"Megalodon", 5467},
            {"Mercanbaligi", 5454},
            {"Moat Carp", 4401},
            {"Mola Mola", 5134},
            {"Monke-Onke", 4462},
            {"Moorish Idol", 5121},
            {"Morinabaligi", 5462},
            {"Muddy Siredon", 5126},
            {"Mussel", 5949},
            // N
            {"Nebimonite", 4361},
            {"Noble Lady", 4485},
            {"Nosteau Herring", 4482},
            // O
            {"Ogre Eel", 4481},
            // P
            // Pamtam Kelp in items
            {"Pearlscale", 5714},
            {"Pelazoea", 5815},
            {"Phanauet Newt", 5125},
            {"Pipira", 4464},
            {"Pirarucu", 5470},
            {"Pterygotus", 5133},
            // Q
            {"Quus", 4514},
            {"Quus x2", 4514},
            {"Quus x3", 4514},
            // R
            {"Red Bubble-Eye", 5446},
            {"Red Terrapin", 4402},
            {"Rhinochimera", 5135},
            {"Ryugu Titan", 4305},
            // S
            {"Sandfish", 4291},
            {"Sandfish x2", 4291},
            {"Sandfish x3", 4291},
            {"Sazanbaligi", 5459},
            {"Sea Zombie", 4475},
            {"Shall Shell", 4484},
            {"Shen", 5997},
            {"Shining Trout", 4354},
            {"Silver Shark", 4451},
            {"Soryu", 5537},
            // T
            {"Takitaro", 4463},
            {"Tavnazian Goby", 5130},
            {"Three-Eyed Fish", 4478},
            {"Tiger Cod", 4483},
            {"Tiger Shark", 5817},
            {"Tiny Goldfish", 4310},
            {"Tiny Goldfish x2", 4310},
            {"Tiny Goldfish x3", 4310},
            {"Titanic Sawfish", 5120},
            {"Titanictus", 4476},
            {"Tricolored Carp", 4426},
            {"Tricorn", 4319},
            {"Trilobite", 4317},
            {"Trumpet Shell", 5466},
            {"Turnabaligi", 5137},
            // Tropical Clam can't be fished up
            // U
            {"Uskumru", 5452},
            // V
            {"Veydal Wrasse", 5141},
            {"Vongola Clam", 5131},
            // Y
            {"Yayinbaligi", 5463},
            {"Yellow Globe", 4403},
            {"Yellow Globe x2", 4403},
            {"Yellow Globe x3", 4403},
            {"Yilanbaligi", 5458},
            // Z
            {"Zafmlug Bass", 4385},
            {"Zebra Eel", 4288}
        };
        #endregion

        #region RodDictionary
        public static Dictionary<string, int> rodDictionary = new Dictionary<string, int>()
        {
            {"Ebisu Fishing Rod", 17011},
            {"Lu Shang's F. Rod", 17386},
            {"Bamboo Fish. Rod", 17389},
            {"Carbon Fish. Rod", 17384},
            {"Clothespole", 17383},
            {"Comp. Fishing Rod", 17381},
            {"Fastwater F. Rod", 17388},
            {"Glass Fiber F. Rod", 17385},
            {"Halcyon Rod", 17015},
            {"Hume Fishing Rod", 17014},
            {"Mithran Fish. Rod", 17380},
            {"S.H. Fishing Rod", 17382},
            {"Tarutaru F. Rod", 17387},
            {"Willow Fish. Rod", 17391},
            {"Yew Fishing Rod", 17390},
            {"MMM Fishing Rod", 19319}
        };
        public static List<string> rodList = new List<string>(rodDictionary.Keys);
        #endregion

        #region GearDictionary
        public static Dictionary<string, int> gearDictionary = new Dictionary<string, int>()
        {
            // Body
            {"Angler's Tunica", 13809},
            {"Fisherman's Apron", 14400},
            {"Fisherman's Smock", 11337},
            {"Fisherman's Tunica", 13808},
            // Hands
            {"Angler's Gloves", 14071},
            {"Fisherman's Gloves", 14070},
            // Legs
            {"Angler's Hose", 14293},
            {"Fisherman's Hose", 14292},
            // Feet
            {"Angler's Boots", 14172},
            {"Fisherman's Boots", 14171},
            {"Waders", 14195},
            // Head
            {"Trainee's Spectacles", 11499},
            // Neck
            {"Fisher's Torque", 10925},
            // Waist
            {"Fisher's Rope", 11768},
            {"Fisherman's Belt", 15452},
            // Rings (castable)
            {"Albatross Ring", 15555},
            {"Pelican Ring", 15554},
            {"Penguin Ring", 15556},
            // Rings (static)
            {"Heron Ring", 15846},
            {"Noddy Ring", 11655},
            {"Puffin Ring", 11654},
            {"Seagull Ring", 15845},
            // Earrings
            {"Duchy Earring", 16042},
            {"Empire Earring", 16049},
            {"Federation Earring", 16041},
            {"Kazham Earring", 16046},
            {"Kingdom Earring", 16039},
            {"Mhaura Earring", 16044},
            {"Nashmau Earring", 16050},
            {"Norg Earring", 16047},
            {"Rabao Earring", 16045},
            {"Republic Earring", 16040},
            {"Safehold Earring", 16048},
            {"Selbina Earring", 16043}
        };
        public static List<string> gearList = new List<string>(gearDictionary.Keys);
        public static int bodyIndex = 0;
        public static int bodyCount = 4;
        public static int handsIndex = bodyIndex + bodyCount;
        public static int handsCount = 2;
        public static int legsIndex = handsIndex + handsCount;
        public static int legsCount = 2;
        public static int feetIndex = legsIndex + legsCount;
        public static int feetCount = 3;
        public static int headIndex = feetIndex + feetCount;
        public static int headCount = 1;
        public static int neckIndex = headIndex + headCount;
        public static int neckCount = 1;
        public static int waistIndex = neckIndex + neckCount;
        public static int waistCount = 2;
        public static int ringsIndex = waistIndex + waistCount;
        public static int ringsCount = 7;
        public static int earringsIndex = ringsIndex + ringsCount;
        public static int earringsCount = 11;
        #endregion

        #region BaitDictionary
        public static Dictionary<string, int> baitDictionary = new Dictionary<string, int>()
        {
            //LURES
            {"Fly Lure", 17405},
            {"Frog Lure", 17403},
            {"Lizard Lure", 17401},
            {"Maze Monger Minnow", 19323},
            {"Minnow", 17407},
            {"Robber Rig", 17002},
            {"Rogue Rig", 17398},
            {"Sabiki Rig", 17399},
            {"Shrimp Lure", 17402},
            {"Sinking Minnow", 17400},
            {"Worm Lure", 17404},
            //"LIVE" BAIT
            {"Crayfish Ball", 16997},
            {"Dried Squid", 19324},
            {"Drill Calamary", 17006},
            {"Dwarf Pugil", 17007},
            {"Giant Shell Bug", 17001},
            {"Insect Ball", 16998},
            {"Large MMM Ball", 17009},
            {"Little Worm", 17396},
            {"Lufaise Fly", 17005},
            {"Lugworm", 17395},
            {"Meatball", 17000},
            {"Peeled Crayfish", 16993},
            {"Peeled Lobster", 17394},
            {"Reg. MMM Ball", 17008},
            {"Rotten Meat", 16995},
            {"Sardine Ball", 16996},
            {"Shell Bug", 17397},
            {"Slice of Bluetail", 16992},
            {"Slice of Carp", 16994},
            {"Sliced Cod", 17393},
            {"Sliced Sardine", 17392},
            {"Trout Ball", 16999}
        };
        public static List<string> baitList = new List<string>(baitDictionary.Keys);
        #endregion
    }
}
