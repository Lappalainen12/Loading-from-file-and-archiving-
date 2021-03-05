using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;

namespace Sk6
{
    class Program
    {

        ///Переменная, указывающая на индекс группы
        public static int indexGroup = 1;

        /// Экземпляр класса Stopwatch для работы со временем выполнения
        public static Stopwatch sw = new Stopwatch();

        /// <summary>
        /// Метод с расчётными данными, расчётной логикой, логикой записи в файл
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int[][] GroupsNumbersArr(uint number)
        {
            
            /// Запуск точки отсчёта времени выполнения программы
            sw.Start();


            /// <summary>
            /// Логика расчёта
            /// </summary>
            /// 
            /// Если переданное число ноль, то возвращается пустой список групп
            if (number == 0)
                return null;

            /// Если переданное число единица, то возвращается список групп с одной группой - единицей
            if (number == 1)
                return new int[][] { new int[] { 1 } };

            /// Создание массива для групп
            int[][] groups = new int[(int)Math.Log(number, 2) + 1][];
            groups[0] = new int[] { 1 };

            /// Создание массива чисел содержащего все числа от 1 до заданного
            /// Единица используется как маркер
            /// Вместо удаления элеменов их значение будет приравниваться нулю
            /// После сортировки 1 будет разделять удалённые элементы и оставшиеся
            int[] numbers = new int[number];
            for (int i = 0; i < number; i++)
                numbers[i] = i + 1;

            /// Массив с промежуточными данными
            int[] group = new int[number];

            /// Цикл пока в массиве индекс единицы не последний
            int index1;
            while ((index1 = Array.BinarySearch(numbers, 1)) != number - 1) /// Проверка индекса единицы
            {

                /// Копия элементов в массив группы
                Array.Copy(numbers, group, number);

                int countGroup = 0; /// Количество элементов в группе
                                    /// Перебор элементов группы. i - индекс проверяемого элемента
                for (int i = index1 + 1; i < number; i++)
                {
                    if (group[i] != 0) /// Пропуск удалённых элементов
                    {
                        
                        /// Удаление из группы всех элементов кратных проверяемому, кроме его самого
                        for (int j = i + 1; j < number; j++)
                            if (group[j] % group[i] == 0)
                                group[j] = 0;

                        /// Удаление элемента из массива чисел
                        numbers[i] = 0;
                        /// Счётчик группы увеличивется
                        countGroup++;
                    
                    }
                }

                /// Сортировка массивов после удаления элементов
                Array.Sort(group);
                Array.Sort(numbers);

                /// Создание массива для добавления в группы
                /// и копирование в него значений старше 1
                int[] _gr = new int[countGroup];
                Array.Copy(group, Array.BinarySearch(group, 1) + 1, _gr, 0, countGroup);

                /// Добавление группы в массив групп
                groups[indexGroup] = _gr;
                indexGroup++;

            }

            /// Остановка записи времени выполнения работы программы
            sw.Stop();


            /// <summary>
            /// Запись полученных данных в файл
            /// </summary>
            using (StreamWriter streamWriter = new StreamWriter("GroupsOfNumbers.txt"))
            {

                streamWriter.WriteLine("В результате расчётов получились следующие группы чисел: ");

                streamWriter.WriteLine("\n");

                foreach (int[] itemOne in groups)
                {

                    foreach (int itemTwo in itemOne)
                    {
                        
                        streamWriter.Write(itemTwo + "\t");

                    }

                    streamWriter.WriteLine();
                }
            }

            /// <summary>
            /// Вызов метода с итогами работы программы
            /// </summary>
            ResultData();

            /// <summary>
            /// Архивация записанных данных (логика выбора)
            /// </summary>
            Console.WriteLine("Желаете ли Вы заархивировать полученные данные?");
            {

                string usValue;

                /// Логика выборов
                while (true)
                {

                    string usCompress = Console.ReadLine();

                    /// Если пользователь ввёл верные значения
                    if (usCompress == "Да" || usCompress == "да" || usCompress == "Нет" || usCompress == "нет")
                    {

                        usValue = usCompress;

                        break;
                    }

                    /// Если значения неверные
                    else if (usCompress != "Да" || usCompress != "да" || usCompress != "Нет" || usCompress != "нет")
                    {
                        Console.WriteLine("Введённые данные неверны!" +
                        " Пожалуйста пропишите да или нет, первые буквы могут быть в верхнем регистре.");
                    }
                }

                if (usValue == "Да" || usValue == "да")
                {

                    string compressed = "GroupsOfNumbers.zip";

                    /// Поток для считывания данных
                    using (FileStream stream = new FileStream("GroupsOfNumbers.txt", FileMode.OpenOrCreate))
                    {
                        /// Поток для записи данных
                        using (FileStream fileStream = File.Create(compressed))
                        {
                            /// Поток архивации
                            using (GZipStream gZipStream = new GZipStream(fileStream, CompressionMode.Compress))
                            {

                                stream.CopyTo(gZipStream);

                                Console.WriteLine("\n");

                                Console.WriteLine("Архивация успешно завершена! Результаты: ");

                                Console.WriteLine("\n");

                                Console.WriteLine("Изначальный объём файла: {0}. Текущий объём файла: {1}.",
                                    stream.Length,
                                    fileStream.Length
                                    );

                            }
                        }
                    }
                }

                else if (usValue == "Нет" || usValue == "нет")
                {

                    Console.WriteLine("\n");

                    Console.WriteLine("Работа программы завершена. Спасибо за использование!");
                }

            }
           
            /// Возврат списка групп
            return groups;

        }

        /// <summary>
        /// Вывод результирующих значений (на консоль)
        /// </summary>
        public static void ResultData()
        {

            /// Информация отображаемая на консоль о числе групп чисел
            Console.WriteLine("В данном числе имеется " + indexGroup + " групп.");
           
            /// Выделение отступа
            Console.WriteLine("\n");

            /// Информация отображаемая на консоль о времени работы программы
            Console.WriteLine($"Время работы программы: {sw.ElapsedMilliseconds * 0.001} секунд");

            /// Выделение отступа
            Console.WriteLine("\n");

            /// Общая информация
            Console.WriteLine("Точную информацию о числах в группах, Вы сможете узнать в записанном файле.");


        }

        /// <summary>
        /// Точка входа в программу, получение данных из внешнего источника, их инициализация и обработка
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {

            /// Ссылка на данные с файлом
            string dataFile = File.ReadAllText(@"C:\newWorkDocument.txt");

            /// Конвертация текущих данных в необходимый формат
            uint ConvertDataFile = Convert.ToUInt32(dataFile);

            /* 
               Логическое условие: если пользователь вносит в файл значение более чем
               в 1 миллиард - программа не выполняется, выводится соответствующее сообщение
            */

            if (ConvertDataFile >= 1000000000)
            {
                Console.WriteLine("Вывод данного значения невозможен.");
            }
            
            /// Если же пользователь вносит число меньшее, чем 1 миллиард - текущие данные передаются в расчётный метод
            else
            {
                GroupsNumbersArr(ConvertDataFile);
            }


            

        }

    }
}
