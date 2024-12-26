﻿using FluentAssertions;

using FC.Codeflix.Catalog.Domain.Exceptions;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTest
{
	private readonly CategoryTestFixture _categoryTestFixture;

	public CategoryTest(CategoryTestFixture categoryTestFixture)
		=> _categoryTestFixture = categoryTestFixture;

	[Fact(DisplayName = nameof(Instantiate))]
	[Trait("Domain", "Category - Aggregates")]
	public void Instantiate()
	{
		var validCategory = _categoryTestFixture.GetValidCategory();
		var datetimeBefore = DateTime.Now;

		var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
		var datetimeAfter = DateTime.Now;

		//Assert.NotNull(category);
		//Assert.Equal(validData.Name, category.Name);
		//Assert.Equal(validData.Description, category.Description);
		//Assert.NotEqual(default(Guid), category.Id);
		//Assert.NotEqual(default(DateTime), category.CreatedAt);
		//Assert.True(category.CreatedAt > datetimeBefore);
		//Assert.True(category.CreatedAt < datetimeAfter);
		//Assert.True(category.IsActive);

		category.Should().NotBeNull();
		category.Name.Should().Be(validCategory.Name);
		category.Description.Should().Be(validCategory.Description);
		category.Id.Should().NotBeEmpty();
		category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
		(category.CreatedAt > datetimeBefore).Should().BeTrue();
		(category.CreatedAt < datetimeAfter).Should().BeTrue();
		(category.IsActive).Should().BeTrue();
	}
	
	[Theory(DisplayName = nameof(InstantiateWithIsActive))]
	[Trait("Domain", "Category - Aggregates")]
	[InlineData(true)]
	[InlineData(false)]
	public void InstantiateWithIsActive(bool isActive)
	{
		var validCategory = _categoryTestFixture.GetValidCategory();
		var datetimeBefore = DateTime.Now;
		var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive);
		var datetimeAfter = DateTime.Now;

		//Assert.NotNull(category);
		//Assert.Equal(validData.Name, category.Name);
		//Assert.Equal(validData.Description, category.Description);
		//Assert.NotEqual(default(Guid), category.Id);
		//Assert.NotEqual(default(DateTime), category.CreatedAt);
		//Assert.True(category.CreatedAt > datetimeBefore);
		//Assert.True(category.CreatedAt < datetimeAfter);
		//Assert.Equal(isActive, category.IsActive);

		category.Should().NotBeNull();
		category.Name.Should().Be(validCategory.Name);
		category.Description.Should().Be(validCategory.Description);
		category.Id.Should().NotBeEmpty();
		category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
		(category.CreatedAt > datetimeBefore).Should().BeTrue();
		(category.CreatedAt < datetimeAfter).Should().BeTrue();
		category.IsActive.Should().Be(isActive);
	}

	[Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
	[Trait("Domain", "Category - Aggregates")]
	[InlineData("")]
	[InlineData(null)]
	[InlineData("     ")]
	public void InstantiateErrorWhenNameIsEmpty(string? name)
	{
		//void action() => new DomainEntity.Category(name!, "Category Description");
		//var exception = Assert.Throws<EntityValidationException>(action);
		//Assert.Equal("Name should not be empty or null", exception.Message);
		var validCategory = _categoryTestFixture.GetValidCategory();
		Action action = () => new DomainEntity.Category(name!, validCategory.Description);
		action.Should()
			.Throw<EntityValidationException>()
			.WithMessage("Name should not be empty or null");
	}

	[Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
	[Trait("Domain", "Category - Aggregates")]
	public void InstantiateErrorWhenDescriptionIsNull()
	{
		var validCategory = _categoryTestFixture.GetValidCategory();
		Action action = () => new DomainEntity.Category(validCategory.Name, null!);
		action.Should()
			.Throw<EntityValidationException>()
			.WithMessage("Description should not be null");
	}

	[Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
	[Trait("Domain", "Category - Aggregates")]
	[InlineData("1")]
	[InlineData("12")]
	[InlineData("a")]
	[InlineData("ca")]
	public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
	{
		var validCategory = _categoryTestFixture.GetValidCategory();
		Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);
		action.Should()
			.Throw<EntityValidationException>()
			.WithMessage("Name should be at least 3 characters long");
	}

	[Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
	[Trait("Domain", "Category - Aggregates")]
	public void InstantiateErrorWhenNameIsGreaterThan255Characters()
	{
		var validCategory = _categoryTestFixture.GetValidCategory();
		var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
		Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);
		action.Should()
			.Throw<EntityValidationException>()
			.WithMessage("Name should be less or equal 255 characters long");
	}

	[Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters))]
	[Trait("Domain", "Category - Aggregates")]
	public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
	{
		var validCategory = _categoryTestFixture.GetValidCategory();
		var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());
		Action action = () => new DomainEntity.Category(validCategory.Name, invalidDescription);
		action.Should()
			.Throw<EntityValidationException>()
			.WithMessage("Description should be less or equal 10.000 characters long");
	}

	[Fact(DisplayName = nameof(Activate))]
	[Trait("Domain", "Category - Aggregates")]
	public void Activate()
	{
		var validCategory = _categoryTestFixture.GetValidCategory();
		var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);
		category.Activate();

		category.IsActive.Should().BeTrue();
	}

	[Fact(DisplayName = nameof(Deactivate))]
	[Trait("Domain", "Category - Aggregates")]
	public void Deactivate()
	{
		var validCategory = _categoryTestFixture.GetValidCategory();
		var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
		category.Deactivate();

		category.IsActive.Should().BeFalse();
	}

	[Fact(DisplayName = nameof(Update))]
	[Trait("Domain", "Category - Aggregates")]
	public void Update()
	{
		var validCategory = _categoryTestFixture.GetValidCategory();
		var newValues = new { Name = "New Name", Description = "New Description" };

		validCategory.Update(newValues.Name, newValues.Description);

		validCategory.Name.Should().Be(newValues.Name);
		validCategory.Description.Should().Be(newValues.Description);
	}

	[Fact(DisplayName = nameof(UpdateOnlyName))]
	[Trait("Domain", "Category - Aggregates")]
	public void UpdateOnlyName()
	{
		var validCategory = _categoryTestFixture.GetValidCategory();
		var newValues = new { Name = "New Name"};
		var currentDescription = validCategory.Description;

		validCategory.Update(newValues.Name);

		validCategory.Name.Should().Be(newValues.Name);
		validCategory.Description.Should().Be(currentDescription);
	}

	[Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
	[Trait("Domain", "Category - Aggregates")]
	[InlineData("")]
	[InlineData(null)]
	[InlineData("     ")]
	public void UpdateErrorWhenNameIsEmpty(string? name)
	{
		var validCategory = _categoryTestFixture.GetValidCategory();
		Action action = () => validCategory.Update(name!);
		action.Should()
			.Throw<EntityValidationException>()
			.WithMessage("Name should not be empty or null");
	}

	[Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
	[Trait("Domain", "Category - Aggregates")]
	[InlineData("1")]
	[InlineData("12")]
	[InlineData("a")]
	[InlineData("ca")]
	public void UpdateErrorWhenNameIsLessThan3Characters(string invalidName)
	{
		var validCategory = _categoryTestFixture.GetValidCategory();
		Action action = () => validCategory.Update(invalidName);
		action.Should()
			.Throw<EntityValidationException>()
			.WithMessage("Name should be at least 3 characters long");
	}

	[Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
	[Trait("Domain", "Category - Aggregates")]
	public void UpdateErrorWhenNameIsGreaterThan255Characters()
	{
		var validCategory = _categoryTestFixture.GetValidCategory();
		var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
		Action action = () => validCategory.Update(invalidName);
		action.Should()
			.Throw<EntityValidationException>()
			.WithMessage("Name should be less or equal 255 characters long");
	}

	[Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
	[Trait("Domain", "Category - Aggregates")]
	public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
	{
		var validCategory = _categoryTestFixture.GetValidCategory();
		var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());
		Action action = () => validCategory.Update("Category New Name", invalidDescription);
		action.Should()
			.Throw<EntityValidationException>()
			.WithMessage("Description should be less or equal 10.000 characters long");
	}
}
