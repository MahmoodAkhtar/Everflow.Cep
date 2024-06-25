using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace Everflow.SharedKernal;

public class AutoMoqDataAttribute : AutoDataAttribute
{
    public AutoMoqDataAttribute() : base(() => new Fixture()
        .Customize(new AutoMoqCustomization
        {
            ConfigureMembers = true, GenerateDelegates = true
        })
        .Customize(new SupportMutableValueTypesCustomization()))
    {
    }
}