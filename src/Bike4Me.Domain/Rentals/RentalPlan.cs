namespace Bike4Me.Domain.Rentals;

public class RentalPlan(int days, decimal dailyRate, decimal penaltyPercentage, decimal additionalDailyFee)
{
    public int Days => days;
    public decimal DailyRate => dailyRate;
    public decimal PenaltyPercentage => penaltyPercentage;
    public decimal AdditionalDailyFee => additionalDailyFee;
}