namespace Gerenciamento_Escolar.Models;

public class TimeService
{
    public static bool IsTimePeriodCorrect(TimeOnly startTime, TimeOnly endTime)
    {
        if (startTime >= endTime)
        {
            return false;
        }

        return true;
    }
}