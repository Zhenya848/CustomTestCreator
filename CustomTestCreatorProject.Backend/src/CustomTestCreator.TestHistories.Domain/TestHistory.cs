using CustomTestCreator.SharedKernel;
using CustomTestCreator.SharedKernel.ValueObjects.Id;

namespace CustomTestCreator.TestHistories.Domain;

public class TestHistory : Entity<TestHistoryId>
{
    public Guid CreatorId { get; private set; }
    public Guid TestId { get; private set; }
    
    public int CountOfAllAnswers { get; private set; }
    public int CountOfRightAnswers { get; private set; }
    
    public DateTime PassageTime { get; private set; }
    
    public TestHistory(
        TestHistoryId id, 
        Guid creatorId, 
        Guid testId, 
        int countOfAllAnswers, 
        int countOfRightAnswers, 
        DateTime passageTime) : base(id)
    {
        CreatorId = creatorId;
        TestId = testId;
        
        CountOfAllAnswers = countOfAllAnswers;
        CountOfRightAnswers = countOfRightAnswers;
        
        PassageTime = passageTime;
    }
}