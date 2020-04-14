using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Permissions;

namespace ExceptionHandling//Вариант 14
{//№ 7 Обработка исключений
    partial class Program
    {/*Задание
Дополнить предыдущую лабораторную работу № 6.
Создать иерархию классов исключений (собственных) – 3 и более. 
Сделать наследование пользовательских типов исключений от стандартных классов .Net (например Exception).
Сгенерировать и обработать как минимум пять различных исключительных ситуаций. 
Например, не позволять при инициализации объектов передавать неверные данные, 
обрабатывать ошибки при работе с памятью и ошибки работы с файлами, деление на ноль, неверный индекс, нулевой указатель и т. д.
В конце поставить универсальный обработчик catch.
Обработку исключений вынести в main. При обработке выводить специфическую информацию о месте, 
диагностике и причине исключения. Последним должен быть блок, который отлавливает все исключения (finally).
Добавьте код в одной из функций максрос accert. 
Объясните что он проверяет, как будет выполняться программа в случае не выполнения условия. 
Объясните назначение accert. Далее приведен перечень заданий.*/

        public interface IOrganism
        {
            void GetInfo();
            public abstract string ToString();
            public double GetWeight();
        }
        public class ZooController : ZooContainer<Animals>, IComparable
        {
            public new int Count { get; set; }

            public ZooController() { Count++; }

            public int CompareTo(object obj)// определяет среднее количество животных заданного вида в зоопарке
            {
                ZooController ZC = obj as ZooController;

                if (ZC != null)
                {
                    if (this.Count < ZC.Count)
                    {
                        return -1;
                    }
                    else if (this.Count > ZC.Count)
                    {
                        return 1;
                    }
                    else
                        return 0;
                }
                else
                {
                    throw new Exception("Параметр должен быть типа ZooController!");
                }
            }

            // Вывод списка, класса-контроллера, на консоль
            public static void Listing()
            {
                foreach (var item in ListOfAnimals)
                {
                    Console.WriteLine(item.ToString());
                }
                Console.WriteLine();
            }
            public override double GetAverageWeight(Animals[] A)
            {
                double totalWeight = 0;
                int count = 0;

                foreach (var item in A)
                {
                    totalWeight += item.Weight;
                    count++;
                }
                double AverageWeight = totalWeight / count;

                return AverageWeight;
            }

            public override int GetCount(Animals[] A)
            {
                int сount = 0;
                foreach (var item in A)
                {
                    сount++;
                }
                return сount;
            }

            public override void GetyearOfBirth(Animals[] A)
            {
                foreach (var item in A)
                {
                    Console.WriteLine($"Время создания: {item.Age}");
                }
            }

            public override void MakeLog()
            {
                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var filePath = Path.Combine(desktopPath, "ExceptionHandling");               
                DirectoryInfo dirInf = new DirectoryInfo(filePath);
                if (!dirInf.Exists)
                {
                    dirInf.Create();
                }

                filePath = filePath + @"\Log.txt";
                FileIOPermission FilePermission = new FileIOPermission(FileIOPermissionAccess.AllAccess, filePath);
                FilePermission.Assert();
                StreamWriter TextStream = new StreamWriter(filePath);
                TextStream.WriteLine("This  Log was created on {0}", DateTime.Now);
                TextStream.Close();
            }
        }
        public abstract class ZooContainer<T> : IEnumerable<T> where T : Animals
        {
            public static List<T> ListOfAnimals { get; private set; }
            public int Count { get { return ListOfAnimals.Count; } }

            public ZooContainer()
            {
                ListOfAnimals = new List<T>();
            }
            public T this[int index]
            {
                get
                {
                    if (index < 0 || index >= Count)
                        throw new IndexOutOfRangeException();
                    return ListOfAnimals[index];
                }
                set
                {
                    if (index < 0 || index >= Count)
                        throw new IndexOutOfRangeException();
                    ListOfAnimals[index] = value;
                }
            }
            public T GetByName(string name)
            {
                return
                ListOfAnimals.FirstOrDefault(
                h => string.Compare(h.Name, name,
               StringComparison.InvariantCultureIgnoreCase) == 0);
            }
            public void Add(T animal)
            {
                ListOfAnimals.Add(animal);
            }
            public T Remove(T animal)
            {
                var element = ListOfAnimals.FirstOrDefault(h => h == animal);
                if (element != null)
                {
                    ListOfAnimals.Remove(element);
                    return element;
                }
                throw new NullReferenceException("В экземпляре объекта не задана ссылка на объект!");
            }
            public void Sort()
            {
                ListOfAnimals.Sort();
            }
            public IEnumerator<T> GetEnumerator()
            {
                return ListOfAnimals.GetEnumerator();
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public abstract double GetAverageWeight(Animals[] A);// средний вес животных заданного вида в зоопарке
            public abstract int GetCount(Animals[] A);// количество хищных птиц
            public abstract void GetyearOfBirth(Animals[] A);// вывести всех животных отсортированных по году рождения   
            public abstract void MakeLog();
        }

        public abstract partial class Animals : IComparable<Animals>
        {
            public new abstract string ToString();
            public abstract void ApplyOperation(Animals[] A, AnimalsProperty AP);

            public string Name;
            public float BodyLength;
            public double Weight;

            public DateTime Age { get; set; }

            public Animals() { Name = "disenabled"; BodyLength = 0; Weight = 0; Age = DateTime.Now; }

            public Animals(string Name)
            {
                if (Name != " ")
                    this.Name = Name;
                else throw new MyObjectInitializationException("Имя \"" + Name + "\" не должно быть пустой строкой!");

                BodyLength = 0.0f;
                Weight = 0;
                Age = DateTime.Now;
            }

            public Animals(string Name, float BodyLength)
            {
                if (Name != " ")                
                    this.Name = Name;
                else throw new MyObjectInitializationException("Имя \"" + Name + "\" не должно быть пустой строкой!");
                if (BodyLength >= 0.0f)
                    this.BodyLength = BodyLength;
                else throw new MyObjectInitializationException("Длина тела животного \"" + BodyLength + "\" не может быть отрицательным числом!");

                Weight = 0;
                Age = DateTime.Now;
            }
            public Animals(string Name, float BodyLength, double Weight)
            {
                if (Name != " ")
                    this.Name = Name;
                else throw new MyObjectInitializationException("Имя \"" + Name + "\" не должно быть пустой строкой!");
                if (BodyLength >= 0.0f)
                    this.BodyLength = BodyLength;
                else throw new MyObjectInitializationException("Длина тела животного \"" + BodyLength + "\" не может быть отрицательным числом!");
                if (Weight >= 0)
                    this.Weight = Weight;
                else throw new MyObjectInitializationException("Вес тела животного \"" + Weight + "\" не может быть отрицательным числом!");
                Age = DateTime.Now;
            }

            public virtual double GetWeight()
            {
                return Weight;
            }

            public static ref double Find(double Weight, Animals[] A)
            {
                for (int i = 0; i < A.Length; i++)
                {
                    if (A[i].Weight == Weight)
                    {
                        return ref A[i].Weight; // возвращается ссылка на адрес, а не само значение
                    }
                }
                throw new IndexOutOfRangeException("Животное с таким весом не найдено!");
            }

            public int CompareTo(Animals other)
            {
                return string.Compare(other.Name, Name, StringComparison.InvariantCultureIgnoreCase);
            }

            struct Animal
            {
                public string Name;
                public float BodyLength;
                public int Weight;

                public string FullInfo
                {
                    get { return string.Format("Название: {0} Длина тела: {1} Вес: {2}", Name, BodyLength, Weight); }
                }
                public Animal(string Name)
                {
                    if (Name != " ")
                        this.Name = Name;
                    else throw new MyObjectInitializationException("Имя \"" + Name + "\" не должно быть пустой строкой!");

                    BodyLength = 0;
                    Weight = 0;
                }
                public Animal(string Name, float BodyLength)
                {
                    if (Name != " ")
                        this.Name = Name;
                    else throw new MyObjectInitializationException("Имя \"" + Name + "\" не должно быть пустой строкой!");
                    if (BodyLength >= 0)
                        this.BodyLength = BodyLength;
                    else throw new MyObjectInitializationException("Длина тела животного \"" + BodyLength + "\" не может быть отрицательным числом!");

                    Weight = 0;
                }
                public Animal(string Name, float BodyLength, int Weight)
                {
                    if (Name != " ")
                        this.Name = Name;
                    else throw new MyObjectInitializationException("Имя \"" + Name + "\" не должно быть пустой строкой!");
                    if (BodyLength >= 0)
                        this.BodyLength = BodyLength;
                    else throw new MyObjectInitializationException("Длина тела животного \"" + BodyLength + "\" не может быть отрицательным числом!");
                    if (Weight >= 0)
                        this.Weight = Weight;
                    else throw new MyObjectInitializationException("Вес тела животного \"" + Weight + "\" не может быть отрицательным числом!");
                }

                public void DisplayInfo()
                {
                    Console.WriteLine($"Название : {Name};\t Длина тела = {BodyLength};\t Вес = {Weight}.");
                }
            }
        }
        public class Mammals : Animals, IOrganism
        {
            public new DateTime Age { get; private set; }

            public Mammals(string Name) : base(Name)
            {
                BodyLength = 0;
                Weight = 0;
                Age = DateTime.Now;
            }
            public Mammals(string Name, float BodyLength) : base(Name)
            {
                if (BodyLength >= 0.0f)
                    this.BodyLength = BodyLength;
                else throw new MyObjectInitializationException("Длина тела животного \"" + BodyLength + "\" не может быть отрицательным числом!");
               
                Weight = 0;
                Age = DateTime.Now;
            }
            public Mammals(string Name, float BodyLength, double Weight) : base(Name)
            {
                if (BodyLength >= 0.0f)
                    this.BodyLength = BodyLength;
                else throw new MyObjectInitializationException("Длина тела животного \"" + BodyLength + "\" не может быть отрицательным числом!");
                if (Weight >= 0)
                    this.Weight = Weight;
                else throw new MyObjectInitializationException("Вес тела животного \"" + Weight + "\" не может быть отрицательным числом!");

                Age = DateTime.Now;
            }
            public override double GetWeight()
            {
                return Weight;
            }
            public void GetInfo()
            {
                Console.WriteLine($"Название животного {Name} Длина тела: {BodyLength} Вес: {Weight} Возраст: {Age}");
            }

            public override string ToString()
            {
                return string.Format("Млекопитающее: {0}; Длина тела = {1}; Вес = {2}.", Name, BodyLength, Weight);
            }

            public override void ApplyOperation(Animals[] A, AnimalsProperty AP)
            {
                switch (AP)
                {
                    case AnimalsProperty.PrintNames:
                        foreach (var item in A)
                        {
                            Console.Write($" {item.Name},");
                        }
                        break;
                    case AnimalsProperty.MAXBodyLength:
                        float max = A[0].BodyLength;
                        for (int i = 0; i < A.Length; i++)
                        {
                            if (max < A[i].BodyLength)
                            {
                                max = A[i].BodyLength;
                            }
                        }
                        Console.WriteLine($" Максимальная длина тела: {max}");
                        break;
                    case AnimalsProperty.TotalWeight:
                        double count = 0;
                        foreach (var item in A)
                        {
                            count += item.Weight;
                        }
                        Console.WriteLine($" Общий вес животных равен: {count}");
                        break;
                }
            }
            struct Mammal
            {
                public string MammalName;
                public float BodyLength;
                public int Weight;

                public Mammal(string MammalName)
                {
                    this.MammalName = MammalName;
                    BodyLength = 0;
                    Weight = 0;
                }
                public Mammal(string MammalName, float BodyLength)
                {
                    this.MammalName = MammalName;
                    this.BodyLength = BodyLength;
                    Weight = 0;
                }
                public Mammal(string MammalName, float BodyLength, int Weight)
                {
                    this.MammalName = MammalName;
                    this.BodyLength = BodyLength;
                    this.Weight = Weight;
                }

                public void DisplayInfo()
                {
                    Console.WriteLine($"Название : {MammalName};\t Длина тела = {BodyLength};\t Вес = {Weight}.");
                }
            }
        }
        sealed class Crocodile
        {
            public string Name;
            public float BodyLength;
            public double Weight;

            public Crocodile() { Name = "disenabled"; BodyLength = 0; Weight = 0; }
            public Crocodile(string Name)
            {
                if (Name != " ")
                    this.Name = Name;
                else throw new MyObjectInitializationException("Имя \"" + Name + "\" не должно быть пустой строкой!");

                BodyLength = 0;
                Weight = 0;
            }
            public Crocodile(string Name, float BodyLength)
            {
                if (Name != " ")
                    this.Name = Name;
                else throw new MyObjectInitializationException("Имя \"" + Name + "\" не должно быть пустой строкой!");
                if (BodyLength >= 0.0f)
                    this.BodyLength = BodyLength;
                else throw new MyObjectInitializationException("Длина тела животного \"" + BodyLength + "\" не может быть отрицательным числом!");

                Weight = 0;
            }
            public Crocodile(string Name, float BodyLength, double Weight)
            {
                if (Name != " ")
                    this.Name = Name;
                else throw new MyObjectInitializationException("Имя \"" + Name + "\" не должно быть пустой строкой!");
                if (BodyLength >= 0.0f)
                    this.BodyLength = BodyLength;
                else throw new MyObjectInitializationException("Длина тела животного \"" + BodyLength + "\" не может быть отрицательным числом!");
                if (Weight >= 0)
                    this.Weight = Weight;
                else throw new MyObjectInitializationException("Вес тела животного \"" + Weight + "\" не может быть отрицательным числом!");
            }
            public override string ToString()
            {
                return string.Format("Рептилия: {0}; Длина тела = {1}; Вес = {2}.", Name, BodyLength, Weight);
            }
            public override int GetHashCode()
            {
                return Name.GetHashCode();
            }
            public override bool Equals(Object obj)
            {
                if (obj == null || GetType() != obj.GetType()) return false;

                Crocodile temp = (Crocodile)obj;
                return Name == temp.Name &&
                BodyLength == temp.BodyLength &&
                Weight == temp.Weight;
                //return base.Equals(temp);
            }
            public new static bool ReferenceEquals(Object objA, Object objB)
            {
                return objA == objB;
            }

            // Finalize(), GetType(), Clone()          
            struct Croco
            {
                public string CrocodileName;
                public float BodyLength;
                public double Weight;

                public Croco(string CrocodileName)
                {
                    this.CrocodileName = CrocodileName;
                    BodyLength = 0;
                    Weight = 0;
                }
                public Croco(string CrocodileName, float BodyLength)
                {
                    this.CrocodileName = CrocodileName;
                    this.BodyLength = BodyLength;
                    Weight = 0;
                }
                public Croco(string CrocodileName, float BodyLength, int Weight)
                {
                    this.CrocodileName = CrocodileName;
                    this.BodyLength = BodyLength;
                    this.Weight = Weight;
                }

                public void DisplayInfo()
                {
                    Console.WriteLine($"Название : {CrocodileName}; Длина тела = {BodyLength}; Вес = {Weight} Хэш: {GetHashCode()}.");
                }
            }
        }
        public class Birds : Animals, IOrganism
        {
            public Birds(string Name) : base(Name)
            {
                BodyLength = 0;
                Weight = 0;
            }
            public Birds(string Name, float BodyLength) : base(Name)
            {
                if (BodyLength >= 0.0f)
                    this.BodyLength = BodyLength;
                else throw new MyObjectInitializationException("Длина тела животного \"" + BodyLength + "\" не может быть отрицательным числом!");

                Weight = 0;
            }
            public Birds(string Name, float BodyLength, double Weight) : base(Name)
            {
                if (BodyLength >= 0.0f)
                    this.BodyLength = BodyLength;
                else throw new MyObjectInitializationException("Длина тела животного \"" + BodyLength + "\" не может быть отрицательным числом!");
                if (Weight >= 0)
                    this.Weight = Weight;
                else throw new MyObjectInitializationException("Вес тела животного \"" + Weight + "\" не может быть отрицательным числом!");
            }
            public override double GetWeight()
            {
                return Weight;
            }
            public void GetInfo()
            {
                Console.WriteLine($"Название животного {Name} Длина тела: {BodyLength} Вес: {Weight}");
            }
            public override string ToString()
            {
                return string.Format("Птица: {0}; Длина тела = {1}; Вес = {2}.", Name, BodyLength, Weight);
            }
            public override void ApplyOperation(Animals[] A, AnimalsProperty AP)
            {
                switch (AP)
                {
                    case AnimalsProperty.PrintNames:
                        foreach (var item in A)
                        {
                            Console.Write($" {item.Name},");
                        }
                        break;
                    case AnimalsProperty.MAXBodyLength:
                        float max = A[0].BodyLength;
                        for (int i = 0; i < A.Length; i++)
                        {
                            if (max < A[i].BodyLength)
                            {
                                max = A[i].BodyLength;
                            }
                        }
                        Console.WriteLine($"Максимальная длина тела: {max}");
                        break;
                    case AnimalsProperty.TotalWeight:
                        double count = 0;
                        foreach (var item in A)
                        {
                            count += item.Weight;
                        }
                        Console.WriteLine($"Общий вес животных равен: {count}");
                        break;
                }
            }

            struct Bird
            {
                public string BirdName;
                public float BodyLength;
                public double Weight;

                public Bird(string BirdName)
                {
                    this.BirdName = BirdName;
                    BodyLength = 0;
                    Weight = 0;
                }
                public Bird(string BirdName, float BodyLength)
                {
                    this.BirdName = BirdName;
                    this.BodyLength = BodyLength;
                    Weight = 0;
                }
                public Bird(string BirdName, float BodyLength, int Weight)
                {
                    this.BirdName = BirdName;
                    this.BodyLength = BodyLength;
                    this.Weight = Weight;
                }

                public void DisplayInfo()
                {
                    Console.WriteLine($"Название : {BirdName};\t Длина тела = {BodyLength};\t Вес = {Weight}.");
                }
            }
        }
        public class Fish : Animals, IOrganism
        {
            public Fish(string Name) : base(Name)
            {
                BodyLength = 0;
                Weight = 0;
            }
            public Fish(string Name, float BodyLength) : base(Name)
            {
                if (BodyLength >= 0.0f)
                    this.BodyLength = BodyLength;
                else throw new MyObjectInitializationException("Длина тела животного \"" + BodyLength + "\" не может быть отрицательным числом!");

                Weight = 0;
            }
            public Fish(string Name, float BodyLength, int Weight) : base(Name)
            {
                if (BodyLength >= 0.0f)
                    this.BodyLength = BodyLength;
                else throw new MyObjectInitializationException("Длина тела животного \"" + BodyLength + "\" не может быть отрицательным числом!");
                if (Weight >= 0)
                    this.Weight = Weight;
                else throw new MyObjectInitializationException("Вес тела животного \"" + Weight + "\" не может быть отрицательным числом!");
            }
            public override double GetWeight()
            {
                return Weight;
            }
            public void GetInfo()
            {
                Console.WriteLine($"Название животного {Name} Длина тела: {BodyLength} Вес: {Weight}");
            }
            public override string ToString()
            {
                return string.Format("Рыба: {0}; Длина тела = {1}; Вес = {2}.", Name, BodyLength, Weight);
            }
            public override void ApplyOperation(Animals[] A, AnimalsProperty AP)
            {
                switch (AP)
                {
                    case AnimalsProperty.PrintNames:
                        foreach (var item in A)
                        {
                            Console.Write($" {item.Name},");
                        }
                        break;
                    case AnimalsProperty.MAXBodyLength:
                        float max = A[0].BodyLength;
                        for (int i = 0; i < A.Length; i++)
                        {
                            if (max < A[i].BodyLength)
                            {
                                max = A[i].BodyLength;
                            }
                        }
                        Console.WriteLine($"Максимальная длина тела: {max}");
                        break;
                    case AnimalsProperty.TotalWeight:
                        double count = 0;
                        foreach (var item in A)
                        {
                            count += item.Weight;
                        }
                        Console.WriteLine($"Общий вес животных равен: {count}");
                        break;
                }
            }

            struct fish
            {
                public string FishName;
                public float BodyLength;
                public int Weight;

                public fish(string FishName)
                {
                    this.FishName = FishName;
                    BodyLength = 0;
                    Weight = 0;
                }
                public fish(string FishName, float BodyLength)
                {
                    this.FishName = FishName;
                    this.BodyLength = BodyLength;
                    Weight = 0;
                }
                public fish(string FishName, float BodyLength, int Weight)
                {
                    this.FishName = FishName;
                    this.BodyLength = BodyLength;
                    this.Weight = Weight;
                }

                public void DisplayInfo()
                {
                    Console.WriteLine($"Название : {FishName};\t Длина тела = {BodyLength};\t Вес = {Weight}.");
                }
            }
        }
        static void Main(string[] args)
        {                    
            ZooController AnimalsController = new ZooController();
            
            // Создание массива класса Animals
            Animals[] animals = new Mammals[5];
            animals[0] = new Mammals("Лев", 2.5f, 190);
            animals[1] = new Mammals("Медоед", 0.77f, 16);
            animals[2] = new Mammals("Амурский тигр", 2.7f, 170);
            animals[3] = new Mammals("Гепард", 1.1f, 21);
            animals[4] = new Mammals("Бурый медведь", 2, 500);

            // Добавление в цикле в список, класса-контроллера, массива Animals + вывод массива на консоль   
            Console.WriteLine("Создание массива класса Animals... " +
            "\nДобавление в цикле в список, класса-контроллера, массива Animals и вывод массива на консоль...\n");
            for (int i = 0; i < animals.Length; i++)
            {
                ZooController.ListOfAnimals.Add(animals[i]);
                Console.WriteLine(animals[i].ToString());
            }
            Console.WriteLine();

            // Вывод списка, класса-контроллера, на консоль
            Console.WriteLine("Вывод списка, класса-контроллера, на консоль...\n");
            foreach (var item in AnimalsController)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine();

            // Вывод только имён из массива
            Console.WriteLine("Вывод только имён из массива...");
            foreach (var item in animals)
            {
                item.ApplyOperation(animals, Animals.AnimalsProperty.PrintNames);
                break;
            }
            Console.WriteLine();

            // Вывод максимальной длины тела среди всех животных
            Console.WriteLine("\nВывод максимальной длины тела среди всех животных...");
            foreach (var item in animals)
            {
                item.ApplyOperation(animals, Animals.AnimalsProperty.MAXBodyLength);
                break;
            }
            Console.WriteLine();

            // Вывод общего веса животных из массива
            Console.WriteLine("Вывод общего веса животных из массива...");
            foreach (var item in animals)
            {
                item.ApplyOperation(animals, Animals.AnimalsProperty.TotalWeight);
                break;
            }
            Console.WriteLine();

            // Обращение к элементу списка, класса-контроллера, по имени экземпляра класса Animals
            Console.WriteLine("Обращение к элементу списка, класса-контроллера, по имени экземпляра класса Animals - 'Амурский тигр'...");
            Console.WriteLine(AnimalsController.GetByName("Амурский тигр").ToString());
            Console.WriteLine();

            // Удаление экземпляра класса Animals из списка, класса-контроллера
            Console.WriteLine("Удаление экземпляра класса Animals с именем 'Гепард' из списка, класса-контроллера...");
            Console.WriteLine(AnimalsController.Remove(animals[3]).ToString());
            Console.WriteLine();

            ZooController.Listing();

            // Сортировка списка, класса-контроллера
            Console.WriteLine("Сортировка списка, класса-контроллера... \nОтсортированный список:");
            AnimalsController.Sort();
            Console.WriteLine();
            ZooController.Listing();

            Console.WriteLine(new string('-', 20));

            // Получение среднего веса животных заданного вида в зоопарке
            Console.WriteLine("Получение среднего веса животных заданного вида в зоопарке...");
            Console.WriteLine($"Средний вес животных в зоопарке: {AnimalsController.GetAverageWeight(animals)}");
            Console.WriteLine();

            // Получение количества животных в списке
            Console.WriteLine("Получение количества животных в списке...");
            Console.WriteLine($"Количество животных в списке: {AnimalsController.GetCount(animals)}");
            Console.WriteLine();

            // Вывод всех животных отсортированных по году рождения
            Console.WriteLine("Вывод всех животных отсортированных по году рождения...");
            AnimalsController.GetyearOfBirth(animals);

            Console.WriteLine(new string('-', 50));////////////////////////////////////////////////////////////////////////////////////////
            Console.WriteLine("Обработка ошибок...\n");            

            Crocodile[] crocodile = new Crocodile[3];
            crocodile[0] = new Crocodile("Крокодил1", 7, 600);
            crocodile[1] = new Crocodile("Крокодил2", 6, 500);
            crocodile[2] = new Crocodile("Крокодил3", 5, 400);            

            for (int i = 0; i < crocodile.Length; i++)
                Console.WriteLine(crocodile[i].ToString());
            Console.WriteLine();

            // Обработка ошибки при деление на ноль
            Console.WriteLine("Обработка ошибки при деление на ноль...");
            try
            {
                int x = (int)crocodile[0].BodyLength;
                int y = 0;
                int result = x / y;
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"Попытка делить на ноль!({ex.Message})");
                //throw new DivideByZeroException("Попытка делить на ноль!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }         
            Console.WriteLine();

            // Обработка ошибки при явном преобразовании
            Console.WriteLine("Обработка ошибки при явном преобразовании...");//throw new MyInvalidCastExceptionClass();  

            //// Проверка, является ли переменная F типа Animals
            //float F = 0.2f;
            //Console.WriteLine("\nПроверка, является ли переменная F типа Animals...");
            //if (F is Animals)
            //    Console.WriteLine("Переменная F имеет тип Animals");
            //else
            //    throw new MyInvalidCastExceptionClass("Переменная F не имеет тип Animals");
            Console.WriteLine();

            // Обработка ошибки при неверном индексе
            Console.WriteLine("Обработка ошибки при неверном индексе...");//throw new IndexOutOfRangeException();
            try
            {
                Console.WriteLine(" Создание массива класса 'Animals' с количеством элементов ноль...");
                Animals[] mammals = new Animals[0];
                mammals[0] = new Mammals("Медоед", 0.77f, 16);

                // Получение среднего веса животных заданного вида в зоопарке
                Console.WriteLine("Получение среднего веса животных заданного вида в зоопарке...");
                Console.WriteLine($"Средний вес животных в зоопарке: {AnimalsController.GetAverageWeight(mammals)}");
                Console.WriteLine();
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine($"Индекс находился вне границ массива!({ex.Message})");
                //throw new IndexOutOfRangeException("Индекс находился вне границ массива!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }      
            finally
            {
                Console.WriteLine("\tсоздание массива привело к ошибке!!!");
            }
            Console.WriteLine();

            // Обработка ошибки при нулевом указателе
            Console.WriteLine("Обработка ошибки при нулевом указателе...");
            try
            {
                string foo = null;
                foo.ToUpper();
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine($"В экземпляре объекта не задана ссылка на объект!({ex.Message})");               
                //throw new MyNullReferenceExceptionClass($"В экземпляре объекта не задана ссылка на объект!({ex.Message})");
            }     
            catch (Exception ex)  
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine();

            // Вызов метода Assert, чтобы переопределить проверку безопасности
            Console.WriteLine("Вызов метода Assert, чтобы переопределить проверку безопасности...");
            AnimalsController.MakeLog();


            Console.ReadKey();
        }
    }
    public static class Printer
    {
        public static object iAmPrinting(this string mammals)
        {
            return mammals.ToString();
        }

        public static string ToStr(this string mammals)
        {
            return mammals.ToString();
        }
    }
}

