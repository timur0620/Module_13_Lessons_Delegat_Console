using System.Security.Cryptography.X509Certificates;

namespace Module_13_Lessons_Delegat_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FactoryEvent fk = new FactoryEvent(10);
            fk.ConsoleView();
        }
    }
    class Worker
    {
        public string firsNAme { get; set; }
        public string lastName { get; set; }
        public int age { get; set; }
        public int salary { get; set; }
        public Worker(string firsName, string lastNameName, int age, int salary) 
        {
            this.firsNAme = firsName;
            this.lastName = lastNameName;
            this.age = age;
            this.salary = salary;
        }
        public override string ToString()
        {
            return $"{firsNAme} {lastName} {age} {salary}";
        }

        static List<Worker> GetWorkers(int count = 10)
        {
            var listW = new List<Worker>();
            Random r = new Random();
            for (int i = 0; i < count; i++)
            {
                listW.Add(new Worker($"NAme {i + 1}", $"Last Name{i + 1}", r.Next(0, 20), r.Next(100, 1000)));
            }
            return listW ;          
        }
        public void ConsoleLook()
        {
            List<Worker> workers = GetWorkers();
            workers.FindAll(Search);

            Predicate<Worker> newSearch = new Predicate<Worker>(Search);
            var findWorker = workers.Find(newSearch);

            findWorker = workers.Find(delegate(Worker obj)
            {
                return obj.age > 5;
            });

            findWorker = workers.Find((Worker obj) =>
            {
                return obj.age > 5;
            });

            findWorker = workers.Find(e => e.age > 5);

        }
        private bool Search(Worker obj)
        {
            return obj.age > 5;
        }
    }
    class DelegateExample
    {
        delegate void DelegateName1();
        delegate void DelegateName2(string SomeString);
        delegate string DelegateName3(string SomeString);

        public delegate void NewDelegateTest(string SomeString);

        public void SomeFunc() {} 
        public void SomeFunc2(string st) { }

        public string SomeFunc3(string SomeString) { return "......"; }      
        public void ConsoleView()
        {
            DelegateName1 dn1 = new DelegateName1(SomeFunc);
            DelegateName2 dn2 = new DelegateName2(SomeFunc2);
            DelegateName3 dn3 = new DelegateName3(SomeFunc3);

            SomeFunc();
            dn1();

            SomeFunc2("....");
            dn2("....");

            string st0 = SomeFunc3("...");
            string st1 = dn3("....");
        }
    }
    class OversDelegate
    {
        public void SomeFunc() {}
        public void SomeFunc2(string st) {}
        public void SomeFunc3(int SomeInt) {}
        public int SomeFunc4(int st) { return 0; }
        public bool SomeFunc5(int arg) { return true; }

        delegate void DelegateParams<T>(T obj);

        delegate TOut ReturnDelegate<TIn, TOut>(TIn someArgument);
        delegate bool ReturnDelegateBool<Tin>(Tin arg);

        public void ConsoleView()
        {
            DelegateParams<string> dg2 = new DelegateParams<string>(SomeFunc2);
            DelegateParams<int> dp3 = new DelegateParams<int>(SomeFunc3);

            ReturnDelegate<int, int> dg4 = new ReturnDelegate<int, int>(SomeFunc4);
            ReturnDelegateBool<int> dg5 = new ReturnDelegateBool<int>(SomeFunc5);

            Action action = SomeFunc;
            Action<string> action1 = SomeFunc2;
            Action<int> action2 = SomeFunc3;

            Func<int, int> action3 = SomeFunc4;

            Predicate<int> predicate = SomeFunc5;

    
        }
    }
    class CarEvent
    {       
        public static int count;
        private static Random random;
        static CarEvent()
        {
            count = 0;
            random = new Random();
        }
        public string Mark { get; set; }
        public string Model { get; set; }
        public string Engine { get; set; }


        public static event Action<string, int> CreateCar;
        public CarEvent(string Mark, string Model, string Engine)
        {
            this.Mark = Mark;
            this.Model = Model;
            this.Engine = Engine;

            count++;
            Thread.Sleep(random.Next(1000, 3000));

            CreateCar?.Invoke($"Car create {DateTime.Now.ToShortTimeString()}. Parametr {Mark} {Model} {Engine}", count);
        }
        public override string ToString()
        {
            return $"{Mark} {Model} {Engine}";
        }
    }
    class FactoryEvent
    {
        private int count;
        static private Random random;
        static FactoryEvent()
        {
            random = new Random();          
        }

        public FactoryEvent(int count)
        {
            this.count = count;
            CarEvent.CreateCar += Car_CreateCar;
        }
        private void Car_CreateCar(string Msg, int Number)
        {
            Console.WriteLine($"Factory {Msg}. Result {Number}");
        }
        public List<CarEvent> ToProduce()
        {
            List<CarEvent> cars = new List<CarEvent>();
            for (int i = 0; i < this.count; i++)
            {
                cars.Add(new CarEvent( Mark: $"Mark {random.Next(1, 6)}", 
                                        Model: $"Model {random.Next(100, 1000)}",
                                        Engine: $"Engine { random.Next(100, 1000)}"
                                     ));
            }
            return cars ;
        }

        public void ConsoleView()
        {
            var factory = new FactoryEvent(10);
            var cars = factory.ToProduce();

            foreach (var item in cars)
            {
                Console.WriteLine(item);
            }
        }
    }
    class PeopleEvent
    {
        public string Name { get; set; }
        public event Action<string> CatFood;
        CatEvent cat;

        public PeopleEvent(string Name, CatEvent ConcretCat)
        {
            this.Name = Name;
            this.cat = ConcretCat;
            ConcretCat.MewEvent += async => Console.WriteLine($"{this.Name} go eat the cat {ConcretCat.NickName}");
            
        }
        public void FeedTheCat()
        {
            Console.WriteLine($"{cat.NickName}, ks, ks, ks eat ready");
            CatFood?.Invoke("Niam, niam, niam");
        }
    }
    class CatEvent
    {
        public event Action<string> MewEvent;

        public PeopleEvent ownew;
        public PeopleEvent Ownew
        {
            set { this.ownew = value; this.ownew.CatFood += s => this.ToEat(); }
        }
        public CatEvent(string NickName, string Breed, int Weight)
        {
            this.NickName = NickName;
            this.breed = Breed;
            this.weight = Weight;
        }
        private string breed;
        private int weight;

        private int Weight { get { return this.weight; } }
        public string NickName { get; set; }
        public string Breed { get { return this.breed; } }

        public void ToEat()
        {
            Console.WriteLine($"{NickName} eaiting....");
        }
        public void ToMew()
        {
            Console.WriteLine($"Mew...");
            MewEvent?.Invoke($"Cat{this.NickName} set Mew");
        }
        public override string ToString()
        {
            return $"{NickName} {breed} { Weight}";
        }
        public void ConsoleView()
        {
            var cat = new CatEvent($"Kity", "SomeBreed", 3);
            PeopleEvent robert = new PeopleEvent("Robert", cat);

            cat.ToMew();

            cat.Ownew = robert;

            robert.FeedTheCat();
        }
    }
    public abstract class Content { }
    class Audio: Content { }
    class Document: Content { }
    class Image: Content { }

    public class TwiterMassageArgs
    {
        public string Time { get; set; }
        public string Massege { get; set; }
        public Content[] Objs { get; set; }
    }
    class TwitterUser
    {
        public string Nick { get; set; }
        public TwitterUser(string Nick)
        {
            this.Nick = Nick;
        }
        public void PublicMessage(string Msg)
        {
            var args = new TwiterMassageArgs() 
            {
                Time = DateTime.Now.ToShortTimeString(),
                Massege = Msg
            };

            Console.WriteLine($"Message {Msg} twiting {Nick}\n");
            Post?.Invoke(this, args );
        }
        public void PublicMessage(string Msg, params Content[] Docs)
        {
            var args = new TwiterMassageArgs()
            {
                Time = DateTime.Now.ToShortTimeString(),
                Massege = Msg,
                Objs = Docs
            };

            Console.WriteLine($"Message {Msg} twiting {Nick}\n");
            Post?.Invoke(this, args);
        }
        public event Action<object, TwiterMassageArgs> Post;
        public void Tape(object sender, TwiterMassageArgs e)
        {
            var user = sender as TwitterUser;

            Console.WriteLine($"Line {this.Nick}: {user.Nick} twitting {e.Massege}");
            if (e.Objs != null)
            {
                Console.WriteLine("and ...");
                foreach (var args in e.Objs)
                {
                    Console.WriteLine($"{args.GetType().Name} ");
                }
            }
            Console.WriteLine();
        }
        public void ConsoleView()
        {
            TwitterUser donald = new TwitterUser("Donald");
            donald.PublicMessage("Hello set Donald");

            TwitterUser jone = new TwitterUser("Jone");
            donald.Post += jone.Tape;

            donald.PublicMessage("Hello agane set Donald");

            TwitterUser mike = new TwitterUser("Mike");
            donald.Post += mike.Tape;

            donald.PublicMessage("Hello agane to set Donald");




        }
    }
    

}
