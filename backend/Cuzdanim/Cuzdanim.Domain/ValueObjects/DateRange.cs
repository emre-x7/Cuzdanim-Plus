namespace Cuzdanim.Domain.ValueObjects;

public class DateRange : IEquatable<DateRange>
{
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }

    public DateRange(DateTime startDate, DateTime endDate)
    {
        if (endDate < startDate)
            throw new ArgumentException("Bitiş tarihi başlangıç tarihinden önce olamaz");

        StartDate = startDate;
        EndDate = endDate;
    }

    public int DurationInDays => (EndDate - StartDate).Days;

    public bool Contains(DateTime date)
        => date >= StartDate && date <= EndDate;

    public bool Overlaps(DateRange other)
        => StartDate <= other.EndDate && EndDate >= other.StartDate;

    public bool Equals(DateRange other)
    {
        if (other is null) return false;
        return StartDate == other.StartDate && EndDate == other.EndDate;
    }

    public override bool Equals(object obj) => Equals(obj as DateRange);
    public override int GetHashCode() => HashCode.Combine(StartDate, EndDate);

    public override string ToString() => $"{StartDate:dd/MM/yyyy} - {EndDate:dd/MM/yyyy}";
}