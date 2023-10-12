using System;
using System.Collections.Generic;
using System.Linq;

class Товар
{
    public string Назва { get; set; }
    public decimal Ціна { get; set; }
    public string Опис { get; set; }
    public string Категорія { get; set; }
}

class Користувач
{
    public string Логін { get; set; }
    public string Пароль { get; set; }
    public List<Замовлення> ІсторіяПокупок { get; set; }

    public Користувач(string логін, string пароль)
    {
        Логін = логін;
        Пароль = пароль;
        ІсторіяПокупок = new List<Замовлення>();
    }
}

class Замовлення
{
    public List<Товар> Товари { get; set; }
    public int Кількість { get; set; }
    public decimal ЗагальнаВартість { get; set; }
    public string Статус { get; set; }

    public Замовлення(List<Товар> товари, int кількість, string статус)
    {
        Товари = товари;
        Кількість = кількість;
        Статус = статус;
        ЗагальнаВартість = ОбчислитиЗагальнуВартість();
    }

    private decimal ОбчислитиЗагальнуВартість()
    {
        decimal загальнаВартість = 0;
        foreach (var товар in Товари)
        {
            загальнаВартість += товар.Ціна * Кількість;
        }
        return загальнаВартість;
    }
}

interface ISearchable
{
    List<Товар> Пошук(Dictionary<string, object> критерії);
}

class Магазин : ISearchable
{
    public List<Товар> Товари { get; set; }
    public List<Користувач> Користувачі { get; set; }

    public Магазин()
    {
        Товари = new List<Товар>();
        Користувачі = new List<Користувач>();
    }

    public void ДодатиТовар(Товар товар)
    {
        Товари.Add(товар);
    }

    public void ДодатиКористувача(Користувач користувач)
    {
        Користувачі.Add(користувач);
    }

    public void СтворитиЗамовлення(Користувач користувач, List<Товар> товари, int кількість, string статус)
    {
        var замовлення = new Замовлення(товари, кількість, статус);
        користувач.ІсторіяПокупок.Add(замовлення);
    }

    public List<Товар> Пошук(Dictionary<string, object> критерії)
    {
        var результат = Товари;

        if (критерії.ContainsKey("цiна"))
        {
            var мінімальнаЦіна = Convert.ToDecimal(критерії["цiна"]);
            результат = результат.Where(товар => товар.Ціна <= мінімальнаЦіна).ToList();
        }

        if (критерії.ContainsKey("категорія"))
        {
            var категорія = критерії["категорiя"].ToString();
            результат = результат.Where(товар => товар.Категорія == категорія).ToList();
        }
        return результат;
    }
}

class Program
{
    static void Main()
    {
        var магазин = new Магазин();

        var товар1 = new Товар { Назва = "Товар 1", Ціна = 100, Опис = "Опис товару 1", Категорія = "Категорiя 1" };
        var товар2 = new Товар { Назва = "Товар 2", Ціна = 200, Опис = "Опис товару 2", Категорія = "Категорiя 2" };
        магазин.ДодатиТовар(товар1);
        магазин.ДодатиТовар(товар2);

        var користувач = new Користувач("user1", "password1");
        магазин.ДодатиКористувача(користувач);

        var критеріїПошуку = new Dictionary<string, object>
        {
            { "цiна", 150 },
            { "категорiя", "Категорiя 1" }
        };

        var результатПошуку = магазин.Пошук(критеріїПошуку);

        Console.WriteLine("Результати пошуку:");
        foreach (var товар in результатПошуку)
        {
            Console.WriteLine($"Назва: {товар.Назва}, Цiна: {товар.Ціна}, Категорiя: {товар.Категорія}");
        }
    }
}