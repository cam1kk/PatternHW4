Handler cardValidityChecker = new CardValidityChecker();
Handler pinCodeChecker = new PinCodeChecker();
Handler accountBalanceChecker = new AccountBalanceChecker();
Handler atmBalanceChecker = new AtmBalanceChecker();

cardValidityChecker.SetSuccessor(pinCodeChecker);
pinCodeChecker.SetSuccessor(accountBalanceChecker);
accountBalanceChecker.SetSuccessor(atmBalanceChecker);

Request request = new Request
{
    CardNumber = 123456789,
    Pin = 1234,
    Amount = 200
};

cardValidityChecker.HandleRequest(request);
class Request
{
    public int CardNumber { get; set; }
    public int Pin { get; set; }
    public double Amount { get; set; }
}

abstract class Handler
{
    protected Handler successor;

    public void SetSuccessor(Handler successor)
    {
        this.successor = successor;
    }

    public abstract void HandleRequest(Request request);
}

class CardValidityChecker : Handler
{
    public override void HandleRequest(Request request)
    {
        if (request.CardNumber != 0)
        {
            Console.WriteLine("Карта действительна.");
            if (successor != null)
                successor.HandleRequest(request);
        }
        else
        {
            Console.WriteLine("Неверный номер карты.");
        }
    }
}

class PinCodeChecker : Handler
{
    public override void HandleRequest(Request request)
    {
        if (request.Pin == 1234)
        {
            Console.WriteLine("Пин код верный.");
            if (successor != null)
                successor.HandleRequest(request);
        }
        else
        {
            Console.WriteLine("Неверный Пин код.");
        }
    }
}

class AccountBalanceChecker : Handler
{
    private double accountBalance = 1000;

    public override void HandleRequest(Request request)
    {
        if (accountBalance >= request.Amount)
        {
            Console.WriteLine("Сумма на счету достаточна.");
            if (successor != null)
                successor.HandleRequest(request);
        }
        else
        {
            Console.WriteLine("Недостаточно средств на счету.");
        }
    }
}

class AtmBalanceChecker : Handler
{
    private double atmBalance = 5000;

    public override void HandleRequest(Request request)
    {
        if (atmBalance >= request.Amount)
        {
            Console.WriteLine("Сумма в банкомате достаточна.");
            WithdrawCash(request);
        }
        else
        {
            Console.WriteLine("Недостаточно средств в банкомате.");
        }
    }

    private void WithdrawCash(Request request)
    {
        Console.WriteLine($"Выдано {request.Amount}$");
        atmBalance -= request.Amount;
    }
}