﻿using Bogus;
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
		var value = Faker.Lorem.Word();
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

	[Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
	[Trait("Domain", "DomainValidation - Validation")]
	[MemberData(nameof(GetValuesSmallerThanMin), parameters: 10)]
	public void MinLengthThrowWhenLess(string target, int minLength)
	{
		string fieldName = Faker.Lorem.Word();
		Action action =
			() => DomainValidation.MinLength(target, minLength, fieldName);

		action.Should()
			.Throw<EntityValidationException>()
			.WithMessage($"{fieldName} should not be less than {minLength} characters long");
	}

	public static IEnumerable<object[]> GetValuesSmallerThanMin(int numberOfTests = 5)
	{
		yield return new object[] { "123456", 10 };
		var Faker = new Faker();
		for (int i = 0; i < (numberOfTests - 1); i++)
		{
			var example = Faker.Lorem.Word();
			var minLength = example.Length + (new Random()).Next(1, 20);
			yield return new object[] { example, minLength };
		}
	}

	[Theory(DisplayName = nameof(MinLengthOk))]
	[Trait("Domain", "DomainValidation - Validation")]
	[MemberData(nameof(GetValuesGreaterThanMin), parameters: 10)]
	public void MinLengthOk(string target, int minLength)
	{
		string fieldName = Faker.Lorem.Word();
		Action action =
			() => DomainValidation.MinLength(target, minLength, fieldName);

		action.Should().NotThrow();
	}
	public static IEnumerable<object[]> GetValuesGreaterThanMin(int numberOfTests = 5)
	{
		yield return new object[] { "123456", 6 };
		var Faker = new Faker();
		for (int i = 0; i < numberOfTests; i++)
		{
			var example = Faker.Lorem.Word();
			var minLength = example.Length - (new Random()).Next(1, 5);
			yield return new object[] { example, minLength };
		}
	}
}
