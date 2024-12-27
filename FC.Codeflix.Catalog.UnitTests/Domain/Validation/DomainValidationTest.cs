using Bogus;
using FluentAssertions;
using FC.Codeflix.Catalog.Domain.Validation;
using FC.Codeflix.Catalog.Domain.Exceptions;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Validation;

public class DomainValidationTest
{
	private Faker Faker {  get; set; } = new Faker();

	[Fact(DisplayName = nameof(NotNullOk))]
	[Trait("Domain", "DomainValidation - Validation")]
	public void NotNullOk()
	{
		var value = Faker.Commerce.ProductName();
		Action action = 
			() 	=> DomainValidation.NotNull(value, "Value");
		action.Should().NotThrow();
	}

	[Fact(DisplayName = nameof(ThrowWhenNull))]
	[Trait("Domain", "DomainValidation - Validation")]
	public void ThrowWhenNull()
	{
		string? value = null;
		string fieldName = Faker.Commerce.ProductName();
		Action action = 
			() 	=> DomainValidation.NotNull(value, fieldName);
		action.Should()
			.Throw<EntityValidationException>()
			.WithMessage($"{fieldName} should not be null");
	}

	[Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
	[Trait("Domain", "DomainValidation - Validation")]
	[InlineData("")]
	[InlineData("  ")]
	[InlineData(null)]
	public void NotNullOrEmptyThrowWhenEmpty(string? target)
	{
		string fieldName = Faker.Commerce.ProductName();
		Action action = 
			() => DomainValidation.NotNullOrEmpty(target, fieldName);
		action.Should()
			.Throw<EntityValidationException>()
			.WithMessage($"{fieldName} should not be null or empty");
	}

	[Fact(DisplayName = nameof(NotNullOrEmptyOk))]
	[Trait("Domain", "DomainValidation - Validation")]
	public void NotNullOrEmptyOk()
	{
		string target = Faker.Commerce.ProductName();
		string fieldName = Faker.Commerce.ProductName();

		Action action = 
			() => DomainValidation.NotNullOrEmpty(target, fieldName);
		action.Should().NotThrow();
	}
}
