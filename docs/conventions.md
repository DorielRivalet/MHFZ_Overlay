# Coding Conventions

- [Coding Conventions](#coding-conventions)
  - [Introduction](#introduction)
    - [Purpose of the document](#purpose-of-the-document)
    - [Importance of coding conventions](#importance-of-coding-conventions)
  - [File Organization](#file-organization)
    - [Folder structure](#folder-structure)
    - [Naming conventions for files and folders](#naming-conventions-for-files-and-folders)
    - [Organization of code within files](#organization-of-code-within-files)
  - [Formatting and Indentation](#formatting-and-indentation)
    - [Use of whitespace](#use-of-whitespace)
    - [Indentation style (spaces vs. tabs)](#indentation-style-spaces-vs-tabs)
    - [Line length and wrapping](#line-length-and-wrapping)
    - [Code Cleanup](#code-cleanup)
  - [Comments and Documentation](#comments-and-documentation)
    - [Use of comments to explain code](#use-of-comments-to-explain-code)
    - [Documentation standards (e.g., XML comments)](#documentation-standards-eg-xml-comments)
    - [Guidelines for writing clear and concise comments](#guidelines-for-writing-clear-and-concise-comments)
  - [Naming Conventions](#naming-conventions)
    - [Naming guidelines for variables, methods, classes, etc](#naming-guidelines-for-variables-methods-classes-etc)
    - [Use of meaningful and descriptive names](#use-of-meaningful-and-descriptive-names)
    - [Abbreviation and acronym conventions](#abbreviation-and-acronym-conventions)
  - [Code Styling](#code-styling)
    - [Braces and brackets placement](#braces-and-brackets-placement)
    - [Use of whitespace around operators and keywords](#use-of-whitespace-around-operators-and-keywords)
    - [Alignment and formatting of code blocks](#alignment-and-formatting-of-code-blocks)
  - [Error Handling](#error-handling)
    - [Best practices for exception handling](#best-practices-for-exception-handling)
    - [Use of custom error codes or enums](#use-of-custom-error-codes-or-enums)
    - [Error message guidelines](#error-message-guidelines)
  - [Code Documentation](#code-documentation)
    - [Standards for documenting code](#standards-for-documenting-code)
    - [Use of code headers or summaries](#use-of-code-headers-or-summaries)
    - [Guidelines for documenting function parameters and return values](#guidelines-for-documenting-function-parameters-and-return-values)
  - [Code Reusability and Modularity](#code-reusability-and-modularity)
    - [Design patterns and principles](#design-patterns-and-principles)
    - [Encapsulation and modular code design](#encapsulation-and-modular-code-design)
    - [Guidelines for code reuse and refactoring](#guidelines-for-code-reuse-and-refactoring)
  - [Testing Guidelines](#testing-guidelines)
    - [Unit testing practices](#unit-testing-practices)
    - [Test naming conventions](#test-naming-conventions)
    - [Guidelines for arranging and organizing tests](#guidelines-for-arranging-and-organizing-tests)
  - [Performance Considerations](#performance-considerations)
    - [Efficient coding practices](#efficient-coding-practices)
    - [Use of algorithms and data structures](#use-of-algorithms-and-data-structures)
    - [Performance profiling and optimization techniques](#performance-profiling-and-optimization-techniques)
  - [Version Control](#version-control)
    - [Branching and merging strategies](#branching-and-merging-strategies)
    - [Commit message conventions](#commit-message-conventions)
    - [Guidelines for working with repositories](#guidelines-for-working-with-repositories)
  - [Team Collaboration](#team-collaboration)
    - [Coding conventions as a collaborative tool](#coding-conventions-as-a-collaborative-tool)
    - [Consistency and communication within the team](#consistency-and-communication-within-the-team)
    - [Code review processes and best practices](#code-review-processes-and-best-practices)
  - [SonarCloud Analysis](#sonarcloud-analysis)
  - [Automatic Code Formatter](#automatic-code-formatter)
  - [Conclusion](#conclusion)

## Introduction

### Purpose of the document

The purpose of this document is to provide a set of coding conventions and guidelines for the project. These conventions aim to establish a consistent coding style, promote readability, and facilitate maintainability of the codebase. By following these guidelines, developers can ensure that their code adheres to the established standards, making it easier for team members to understand and collaborate on the project.

### Importance of coding conventions

Coding conventions play a crucial role in software development. They help improve the overall quality of the code by promoting consistency, readability, and maintainability. Consistent code style and structure make it easier for developers to understand and navigate the codebase, resulting in increased productivity and reduced time spent on comprehension. Additionally, adhering to coding conventions enhances code review processes, promotes collaboration among team members, and facilitates the onboarding of new developers into the project.

## File Organization

For more information, please see [the documentation](./README.md#architecture).

### Folder structure

The project follows a specific folder structure to maintain a well-organized codebase. The main folders in the project are as follows:

- **Models**: Contains the model classes that represent data structures and entities in the application. It also includes a subfolder called Collections that holds various collection classes, such as ReadOnlyDictionary, used throughout the project.

- **ViewModels**: Contains the view model classes that handle the logic and data binding between the views and models. This folder may include subfolders like Pages and Windows for organizing the view models specific to different types of views.

- **Services**: Contains the service classes that provide functionality and perform specific tasks within the application. Additionally, it includes a subfolder called Contracts that holds the interfaces defining the contracts for various services.

- **Views**: Contains the view files, including XAML files for user interfaces. This folder may also include subfolders like UserControls for organizing reusable UI components.

- **Assets**: Holds any static files, resources, or assets required by the application, such as images, colors, or sound files.

### Naming conventions for files and folders

To ensure consistency and clarity within the codebase, the following naming conventions should be followed:

- **Files**: File names should be descriptive, concise, and use PascalCase. Each file should represent a single logical unit or component. For example, a view model file for a login page can be named LoginPageViewModel.cs, and a XAML file for a user control can be named UserControlView.xaml.

- **Folders**: Folder names should be in plural form and use PascalCase. Folder names should reflect the purpose or category of the contained files. For example, the folder for view models can be named ViewModels, and the folder for user controls can be named UserControls.

It is important to note that Hungarian notation should be avoided. Instead, focus on clear and meaningful names that accurately represent the purpose or functionality of the files and folders.

In addition to these conventions, the project adheres to the coding style guidelines defined by StyleCop Analyzers and EditorConfig, ensuring consistency in formatting, indentation, and other code style aspects.

### Organization of code within files

To maintain readability and understandability of the code, it is important to follow a consistent organization within files. The following guidelines should be followed:

- **Logical Grouping**: Group related code together to improve cohesion and make it easier to understand the purpose and functionality of each section.

- **Comments**: Use comments to provide explanations, clarify complex code sections, or describe the purpose of a block of code. Follow best practices for writing clear and concise comments.

- **Method and Property Order**: Define methods and properties in a logical order, such as placing constructors at the top, followed by public methods, then protected methods, and finally private methods. Similarly, group related properties together.

## Formatting and Indentation

- **DO**:

```csharp
// Good example - Proper use of whitespace and indentation

public class Calculator
{
    public int Add(int num1, int num2)
    {
        int sum = num1 + num2;

        if (sum > 0)
        {
            Console.WriteLine("The sum is positive.");
        }
        else
        {
            Console.WriteLine("The sum is zero or negative.");
        }

        return sum;
    }
}
```

- **DON'T**:

```csharp
// Bad example - Inconsistent use of whitespace and improper indentation

public class Calculator
{
    public int Add(int num1, int num2)
    {
    int sum= num1+num2;

    if(sum>0)
    {
    Console.WriteLine("The sum is positive.");
    } else
    {
    Console.WriteLine("The sum is zero or negative.");
    }
    
    return sum;
    }
}
```

### Use of whitespace

Whitespace plays an important role in enhancing code readability. The following guidelines should be followed:

- **Spacing**: Use a consistent spacing style throughout the codebase. Typically, one space is used between keywords, operators, and variables, and around binary operators. For example, int sum = num1 + num2;.

- **Vertical Spacing**: Use blank lines to separate logical sections of code, such as methods, properties, or related blocks of code.

### Indentation style (spaces vs. tabs)

The project uses spaces for indentation. Each indentation level should consist of four spaces. This ensures consistent and visually appealing code formatting across different editors and environments.

### Line length and wrapping

To improve code readability, it is recommended to limit the line length to a maximum of 120 characters. If a line exceeds this limit, consider breaking it into multiple lines. Ensure that the line breaks occur at logical points, such as after a comma or operator. Additionally, avoid excessive nesting or deeply nested expressions that may lead to long lines.

### Code Cleanup

Visual Studio provides a helpful feature called Code Cleanup, which automatically applies formatting and style rules defined in the project. Developers can use this feature to ensure their code adheres to the defined coding conventions and formatting guidelines. It can be accessed by right-clicking the solution, expanding Analyze and Code Cleanup and clicking Run Code Cleanup.

By regularly utilizing the Code Cleanup option, developers can maintain a consistent and standardized codebase, improving readability and collaboration within the team.

## Comments and Documentation

- **DO**:

```csharp
// Good example - Clear and concise comments

public class Calculator
{
    /// <summary>
    /// Adds two numbers and returns the sum.
    /// </summary>
    /// <param name="num1">The first number.</param>
    /// <param name="num2">The second number.</param>
    /// <returns>The sum of the two numbers.</returns>
    public int Add(int num1, int num2)
    {
        // Check if the sum is positive
        int sum = num1 + num2;

        if (sum > 0)
        {
            Console.WriteLine("The sum is positive.");
        }
        else
        {
            Console.WriteLine("The sum is zero or negative.");
        }

        return sum;
    }
}
```

- **DON'T**:

```csharp
// Bad example - Redundant and unclear comments

public class Calculator
{
    /// <summary>
    /// This method adds two numbers.
    /// </summary>
    /// <param name="num1">The first number.</param>
    /// <param name="num2">The second number.</param>
    /// <returns>The sum.</returns>
    public int Add(int num1, int num2)
    {
        // Add num1 and num2 and store the result in sum
        int sum = num1 + num2;

        if (sum > 0)
        {
            Console.WriteLine("The sum is positive.");
        }
        else
        {
            Console.WriteLine("The sum is zero or negative.");
        }

        return sum;
    }
}
```

### Use of comments to explain code

Comments are a valuable tool for providing explanations and clarifications within the code. They should be used judiciously to enhance code understanding. The following guidelines should be followed:

- **Commenting Purpose**: Use comments to explain the intent, logic, or functionality of the code. Focus on providing insights that are not immediately evident from reading the code itself.

- **Avoid Redundant Comments**: Avoid commenting obvious or self-explanatory code. Comments should provide additional context or clarify complex sections.

- **Commented Out Code**: Minimize the use of commented out code. In general, commented out code should be removed as it can make the codebase cluttered and trigger [SonarCloud](https://sonarcloud.io/project/overview?id=DorielRivalet_MHFZ_Overlay) code smells. If necessary, consider using version control systems to keep track of code changes instead of leaving commented out code in the files.

### Documentation standards (e.g., XML comments)

The project follows the standard practice of using XML comments to document the public API of classes, methods, and properties. XML comments provide a structured and standardized way to document the purpose, usage, and parameters of code elements. Developers should adhere to the following guidelines:

- **XML Comment Format**: Use the standard XML comment format, starting with ///, preceding the code element to be documented. Include relevant tags such as `<summary>`, `<param>`, `<returns>`, etc., to provide detailed information. These comments are called [docblocks](https://en.wikipedia.org/wiki/Docblock).

- **Describe Parameters and Return Values**: Document the purpose, expected values, and usage of method parameters. Clearly define the return values and any special considerations.

- **Keep Documentation Up to Date**: Ensure that the XML comments are kept up to date with the corresponding code. When modifying a method or class, update the associated documentation to reflect the changes.

### Guidelines for writing clear and concise comments

Clear and concise comments improve code understanding and maintainability. Follow these guidelines when writing comments:

- **Use Clear and Descriptive Language**: Write comments using clear and descriptive language. Avoid using abbreviations or unclear terminology.

- **Avoid Ambiguous or Misleading Comments**: Ensure that comments accurately reflect the code's functionality. Avoid comments that could lead to confusion or misinterpretation.

- **Regular Maintenance**: Regularly review and update comments to keep them relevant and in sync with the code. Outdated or misleading comments can create confusion and lead to incorrect assumptions.

## Naming Conventions

- **DO**:

```csharp
// Good example - Meaningful and descriptive names

public class CustomerService
{
    public void CreateNewCustomer(string firstName, string lastName)
    {
        // Implementation code
    }
    
    public decimal CalculateTotalOrderAmount(decimal unitPrice, int quantity)
    {
        // Implementation code
        return unitPrice * quantity;
    }
    
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
    }
}
```

- **DON'T**:

```csharp
// Bad example - Inconsistent and unclear names

public class CS
{
    public void AddCust(string fName, string lName)
    {
        // Implementation code
    }
    
    public decimal GetAmt(decimal uPrice, int qty)
    {
        // Implementation code
        return uPrice * qty;
    }
    
    public class O
    {
        public int Id { get; set; }
        public DateTime Dt { get; set; }
        public string Status { get; set; }
    }
}
```

### Naming guidelines for variables, methods, classes, etc

Consistent and meaningful naming conventions enhance code readability and maintainability. Follow these guidelines when naming code elements:

- **PascalCase**: Use PascalCase for class names, method names, and property names. The first letter of each word should be capitalized, including the first word.

- **camelCase**: Use camelCase for variable names and parameter names. The first letter of the first word should be lowercase, and the first letter of subsequent words should be capitalized.

- **Avoid Underscores**: Avoid using underscores (_) in names unless necessary for clarity. Replace SNAKE_CASE with PascalCase.

### Use of meaningful and descriptive names

Choose names that accurately reflect the purpose and functionality of the code element. Aim for clarity and avoid ambiguous or generic names. Consider the following guidelines:

- **Be Explicit**: Use descriptive names that clearly convey the intent and purpose of the code element. Avoid vague or generic names.

- **Avoid Single-letter Names**: Except for loop counters or well-known conventions, avoid single-letter names. Instead, use meaningful names that indicate the purpose of the variable or method.

- **Avoid Abbreviations**: Prefer full words or phrases over abbreviations. Clear and understandable names improve code comprehension.

- **Use Domain Terminology**: When appropriate, use domain-specific terminology that aligns with the problem domain or business context.

### Abbreviation and acronym conventions

Abbreviations and acronyms should be used sparingly and only when they are well-known and widely understood. Follow these guidelines:

- **Avoid Cryptic Abbreviations**: Avoid using abbreviations that are not widely recognized or may be ambiguous. Favor readability and clarity over brevity.

- **Use Standard Acronyms**: When using acronyms, ensure they are commonly known and widely used in the industry or domain. If necessary, provide a comment or documentation to explain less-known acronyms.

- **Consistent Capitalization**: Maintain consistent capitalization for abbreviations and acronyms. Use all uppercase or lowercase letters, depending on the convention established for the specific term.

Remember, the goal of naming conventions is to make the code more readable and understandable. Choose names that convey meaning and purpose, and ensure consistency throughout the codebase.

## Code Styling

Consistent code styling improves code readability and maintainability. Follow these guidelines for code styling:

### Braces and brackets placement

- **Braces for Control Statements**: Place the opening brace ({) on the same line as the control statement (e.g., if, for, while). Place the closing brace (}) on a new line, aligned with the control statement.

```csharp
if (condition)
{
    // Code block
}
```

- **Braces for Methods and Classes**: Place the opening brace ({) on the same line as the method or class declaration. Place the closing brace (}) on a new line, aligned with the method or class declaration.

```csharp
public class MyClass
{
    // Code block
}

public void MyMethod()
{
    // Code block
}
```

### Use of whitespace around operators and keywords

- **Binary Operators**: Add a single space before and after binary operators (=, +, -, *, /, etc.) for improved readability.

```csharp
int result = x + y;
```

- **Unary Operators**: Do not use spaces between unary operators (!, ++, --) and the operand.

```csharp
bool isTrue = !isValid;
```

- **Keywords**: Use a single space after keywords (if, while, for, etc.) for clarity.

```csharp
if (condition)
{
    // Code block
}
```

### Alignment and formatting of code blocks

- **Indentation**: Use consistent indentation with four spaces for each level of indentation. Do not use tabs.

```csharp
public void MyMethod()
{
    if (condition)
    {
        // Code block
        if (nestedCondition)
        {
            // Code block
        }
    }
}
```

- **Vertical Spacing**: Use a blank line to separate logical sections of code for improved readability.

```csharp
public void MyMethod()
{
    // Code block

    // Blank line

    // Code block
}
```

Remember to configure your editor or IDE to enforce these code styling conventions automatically. Many code editors have built-in features or extensions to format code according to a predefined style, such as EditorConfig or IDE-specific settings. Additionally, leverage tools like code formatters and linters to maintain consistent code styling throughout the project. For this, we recommend StyleCop Analyzers.

## Error Handling

For more information, see [here](./logging.md).

Proper error handling is essential for robust and reliable software. Follow these guidelines for error handling:

### Best practices for exception handling

- **Catch Specific Exceptions**: Catch specific exceptions rather than general ones to handle different error scenarios effectively.

```csharp
try
{
    // Code that may throw an exception
}
catch (FileNotFoundException ex)
{
    // Handle file not found exception
}
catch (DivideByZeroException ex)
{
    // Handle divide by zero exception
}
catch (Exception ex)
{
    // Handle general exception (fallback)
}
```

- **Avoid Catching and Swallowing Exceptions**: Avoid catching exceptions without proper handling or logging, as it can hide critical errors. Only catch exceptions when you have a meaningful way to handle them.

```csharp
try
{
    // Code that may throw an exception
}
catch (Exception ex)
{
    // Log the exception and rethrow it
    logger.Error(ex);
    throw;
}
```

### Use of custom error codes or enums

*To be determined*.

### Error message guidelines

- **Clear and Descriptive**: Error messages should be clear, concise, and provide enough information to understand the cause of the error. Avoid cryptic or generic error messages.

- **Include Relevant Context**: Include relevant information such as the input data, the specific operation, or the failed component in the error message to aid troubleshooting.

- **Avoid Sensitive Information**: Ensure that error messages do not disclose sensitive information or expose implementation details that could be exploited by malicious actors.

- **Localize Error Messages**: for supporting multiple languages, consider localizing error messages to provide a better user experience.

Remember to handle exceptions at the appropriate level in the application and provide meaningful feedback to users when errors occur.

## Code Documentation

Proper code documentation is crucial for understanding and maintaining the software. Follow these guidelines for code documentation:

### Standards for documenting code

- **XML Documentation Comments**: Use XML documentation comments to document classes, methods, properties, and other code elements. This enables tools like IntelliSense to provide contextual help and improves code readability.

```csharp
/// <summary>
/// Represents a class that performs a specific operation.
/// </summary>
public class MyClass
{
    /// <summary>
    /// Performs the specified operation.
    /// </summary>
    /// <param name="input">The input value.</param>
    /// <returns>The result of the operation.</returns>
    public int PerformOperation(int input)
    {
        // Method implementation
    }
}
```

- **Consistent Formatting**: Follow a consistent formatting style for your documentation comments, including indentation, line breaks, and tag placement. This enhances readability and maintains a professional appearance.

### Use of code headers or summaries

- **Class Summaries**: Include a summary comment at the beginning of each class file to provide an overview of the class's purpose, functionality, and key features.

- **Method Summaries**: Add a summary comment above each method to describe its purpose, input parameters, return value, and any important considerations.

### Guidelines for documenting function parameters and return values

- **Parameter Documentation**: Document each method's input parameters, describing their purpose, expected values, and any constraints or requirements.

```csharp
/// <summary>
/// Calculates the area of a rectangle.
/// </summary>
/// <param name="length">The length of the rectangle.</param>
/// <param name="width">The width of the rectangle.</param>
/// <returns>The calculated area of the rectangle.</returns>
public double CalculateArea(double length, double width)
{
    // Method implementation
}
```

- **Return Value Documentation**: Describe the return value of a method, including its purpose, data type, and any special cases or conditions.

```csharp
/// <summary>
/// Divides two numbers and returns the result.
/// </summary>
/// <param name="numerator">The numerator.</param>
/// <param name="denominator">The denominator.</param>
/// <returns>The result of the division.</returns>
public double DivideNumbers(double numerator, double denominator)
{
    // Method implementation
}
```

Remember to keep your code documentation up to date as you make changes to the code. Well-documented code helps improve readability, maintainability, and collaboration among developers.

## Code Reusability and Modularity

For more information, see [here](./classes.md).

- **DO**:

```csharp
// Good example - Code reusability and modularity

// Design patterns and principles
public interface ILogger
{
    void Log(string message);
}

public class FileLogger : ILogger
{
    public void Log(string message)
    {
        // Implementation code
        // Log message to a file
    }
}

// Encapsulation and modular code design
public class ProductService
{
    private readonly ILogger _logger;

    public ProductService(ILogger logger)
    {
        _logger = logger;
    }

    public void ProcessProduct(Product product)
    {
        // Implementation code
        // Perform product processing logic

        _logger.Log("Product processed: " + product.Name);
    }
}

// Guidelines for code reuse and refactoring
public static class MathUtils
{
    public static int Add(int a, int b)
    {
        return a + b;
    }
}
```

- **DON'T**:

```csharp
// Bad example - Lack of code reusability and modularity

public class ProductService
{
    public void ProcessProduct(Product product)
    {
        // Implementation code
        // Perform product processing logic

        // Logging code directly inside the method
        File.WriteAllText("log.txt", "Product processed: " + product.Name);
    }
}

public class MathUtils
{
    public static int Add(int a, int b)
    {
        // Implementation code
        // Duplicate addition logic
        return a + b;
    }
}
```

To promote code reusability and modularity, follow these guidelines:

### Design patterns and principles

- **Design Pattern Usage**: Familiarize yourself with commonly used design patterns and apply them appropriately in your code. Design patterns provide proven solutions to recurring design problems and help improve code structure and maintainability.

- **SOLID Principles**: Follow the SOLID principles (Single Responsibility, Open-Closed, Liskov Substitution, Interface Segregation, and Dependency Inversion) to design classes and components that are easy to understand, maintain, and extend.

### Encapsulation and modular code design

- **Encapsulation**: Encapsulate related data and behavior within classes or modules. Use access modifiers (e.g., private, protected, internal) to control the visibility and accessibility of class members.

- **Modular Code**: Organize your code into modules or components that have well-defined responsibilities and boundaries. This promotes code separation, reusability, and easier maintenance.

### Guidelines for code reuse and refactoring

- **Reuse Existing Code**: Before writing new code, check if there are existing code snippets, libraries, or modules that can be reused. Reusing code reduces duplication, saves development time, and promotes consistency.

- **Refactor for Reusability**: When refactoring code, identify opportunities to extract reusable components or functions. Extracting common functionality into reusable modules or utility classes improves code maintainability and reduces duplication.

- **Code Abstraction**: Abstract common functionality into generic or abstract classes, interfaces, or base classes. This allows for easier customization and extension in different parts of the codebase.

- **Keep Code DRY**: Follow the "Don't Repeat Yourself" principle (DRY) and avoid duplicating code logic. Duplication increases the risk of inconsistencies and makes code maintenance more difficult.

By following these guidelines, you can create code that is modular, reusable, and easy to maintain, leading to improved development productivity and software quality. Remember: reduce, reuse, and recycle.

## Testing Guidelines

For more information, see [here](./testing.md).

- **DO**:

```csharp
// Good example - Testing guidelines

// Unit testing practices
[TestClass]
public class ProductServiceTests
{
    [TestMethod]
    public void ProcessProduct_ShouldLogProductProcessed()
    {
        // Arrange
        var loggerMock = new Mock<ILogger>();
        var productService = new ProductService(loggerMock.Object);
        var product = new Product { Name = "Test Product" };

        // Act
        productService.ProcessProduct(product);

        // Assert
        loggerMock.Verify(l => l.Log("Product processed: " + product.Name), Times.Once);
    }
}

// Test naming conventions
[TestClass]
public class MathUtilsTests
{
    [TestMethod]
    public void Add_ShouldReturnCorrectSum()
    {
        // Arrange
        int a = 5;
        int b = 3;

        // Act
        int sum = MathUtils.Add(a, b);

        // Assert
        Assert.AreEqual(8, sum);
    }
}
```

- **DON'T**:

```csharp
// Bad example - Lack of testing guidelines

public class ProductService
{
    public void ProcessProduct(Product product)
    {
        // Implementation code
        // Perform product processing logic

        File.WriteAllText("log.txt", "Product processed: " + product.Name);
    }
}

public class MathUtils
{
    public static int Add(int a, int b)
    {
        return a + b;
    }
}
```

To ensure the quality and reliability of your code, follow these testing guidelines:

### Unit testing practices

- **Test-Driven Development (TDD)**: Consider adopting the practice of TDD, where you write tests before implementing the corresponding functionality. TDD helps in designing modular and testable code from the start.

- **Isolation of Dependencies**: Unit tests should focus on testing individual units of code in isolation. Mock or stub external dependencies to isolate the unit under test and ensure that failures are due to issues in the unit being tested.

- **Test Coverage**: Aim for a high test coverage to ensure that critical parts of your code are thoroughly tested. Use tools like SonarCloud to track code coverage and identify areas that require additional testing.

### Test naming conventions

- **Descriptive Names**: Use descriptive and meaningful names for your test methods that clearly indicate the purpose of the test.

- **Consistent Naming Convention**: Follow a consistent naming convention for your test methods. Consider using a prefix like "Test" or suffix like "Should" to distinguish test methods from regular code.

### Guidelines for arranging and organizing tests

- **Arrange-Act-Assert (AAA) Pattern**: Structure your tests using the AAA pattern, where you clearly separate the Arrange (setup), Act (execution), and Assert (verification) phases of the test. This improves the readability and maintainability of your tests.

- **Test Organization**: Organize your tests into logical groups based on the features, components, or classes being tested. Use test classes or test fixtures to group related tests together.

- **Test Initialization**: Use setup methods or fixtures to set up common test data or initialize test objects before running multiple test methods. This avoids code duplication and promotes code reuse.

- **Test Cleanup**: Ensure proper cleanup of resources and test data after each test execution. Use teardown methods or fixtures to perform necessary cleanup tasks.

By following these testing guidelines, you can improve the reliability and stability of your code, catch bugs early, and build confidence in the functionality of the software.

## Performance Considerations

For more information, see [here](./performance.md).

- **DO**:

```csharp
// Good example - Performance considerations

// Efficient coding practices
public class ObjectPoolingExample
{
    private ObjectPool<MyObject> objectPool = new ObjectPool<MyObject>();

    public void ProcessData(List<Data> dataList)
    {
        foreach (Data data in dataList)
        {
            MyObject obj = objectPool.AcquireObject();
            // Perform data processing using obj
            objectPool.ReleaseObject(obj);
        }
    }
}

public class StringBuilderExample
{
    public string ConcatenateStrings(List<string> strings)
    {
        StringBuilder sb = new StringBuilder();
        foreach (string str in strings)
        {
            sb.Append(str);
        }
        return sb.ToString();
    }
}

// Use of algorithms and data structures
public class QuickSortExample
{
    public void SortArray(int[] array)
    {
        QuickSort(array, 0, array.Length - 1);
    }

    private void QuickSort(int[] array, int low, int high)
    {
        if (low < high)
        {
            int pivotIndex = Partition(array, low, high);
            QuickSort(array, low, pivotIndex - 1);
            QuickSort(array, pivotIndex + 1, high);
        }
    }

    private int Partition(int[] array, int low, int high)
    {
        int pivot = array[high];
        int i = low - 1;
        for (int j = low; j < high; j++)
        {
            if (array[j] < pivot)
            {
                i++;
                Swap(array, i, j);
            }
        }
        Swap(array, i + 1, high);
        return i + 1;
    }

    private void Swap(int[] array, int i, int j)
    {
        int temp = array[i];
        array[i] = array[j];
        array[j] = temp;
    }
}

// Performance profiling and optimization techniques
public class PerformanceProfilerExample
{
    public void RunPerformanceTest()
    {
        // Run code segment to be profiled

        // Use a performance profiler to analyze execution times and identify bottlenecks
    }

    public void MeasureExecutionTime()
    {
        DateTime startTime = DateTime.Now;

        // Code segment to be measured

        TimeSpan executionTime = DateTime.Now - startTime;
        Console.WriteLine("Execution time: " + executionTime.TotalMilliseconds + "ms");
    }
}
```

- **DON'T**:

```csharp
// Bad example - Lack of performance considerations

public class InefficientCodeExample
{
    public void ProcessData(List<Data> dataList)
    {
        foreach (Data data in dataList)
        {
            // Perform data processing
        }
    }

    public string ConcatenateStrings(List<string> strings)
    {
        string result = "";
        foreach (string str in strings)
        {
            result += str;
        }
        return result;
    }

    public void SortArray(int[] array)
    {
        // Use an inefficient sorting algorithm like Bubble Sort
    }

    public void RunPerformanceTest()
    {
        // Run code segment without any profiling or measurement
    }
}
```

To optimize the performance of your code, consider the following guidelines:

### Efficient coding practices

- **Minimize Object Creation**: Avoid unnecessary object creation, especially within performance-critical sections of your code. Reuse objects when possible and prefer object pooling techniques.

- **Avoid String Concatenation**: String concatenation can be expensive in terms of memory and performance. Instead, use StringBuilder for concatenating large strings or use string interpolation for simpler cases.

- **Optimize Loops**: Review your loops and minimize unnecessary iterations. Use efficient loop constructs such as for loops instead of foreach loops when the index is required.

### Use of algorithms and data structures

- **Choose the Right Data Structure**: Select appropriate data structures based on the requirements of your code. Use data structures with efficient search, insert, and delete operations to optimize performance.

- **Optimize Sorting**: When sorting large collections, choose efficient sorting algorithms such as QuickSort or MergeSort. Consider using built-in sorting functions or libraries that offer optimized sorting algorithms.

- **Cache Data**: Cache frequently accessed data or expensive computations to avoid redundant calculations. Caching can significantly improve performance, especially in scenarios where data is expensive to retrieve or compute.

### Performance profiling and optimization techniques

- **Performance Profiling**: Use profiling tools to identify performance bottlenecks in your code. Profilers can help pinpoint areas of code that consume the most CPU or have high memory usage, allowing you to focus your optimization efforts.

- **Measure and Benchmark**: Establish performance benchmarks and measure the execution time of critical code sections. Continuously monitor and compare the performance to ensure that optimizations have a positive impact.

- **Optimize Hot Paths**: Focus your optimization efforts on critical code paths that are frequently executed or consume a significant portion of system resources. Optimize these sections to gain the most significant performance improvements.

By following these performance considerations and applying optimization techniques, you can enhance the efficiency and responsiveness of your code, leading to improved overall system performance.

## Version Control

For more information, see [here](./deployment.md).

- **DO**:

```text
// Good example - Version Control

// Branching and merging strategies:
// Follows specified branching model
// Feature branches
// Release branches
// Hotfix branches
// Proper branch naming conventions
git checkout -b feature/my-feature
git checkout -b release/v1.0
git checkout -b hotfix/bug-fix

// Commit message conventions:
// Clear and descriptive messages
// Conventional commit format
git commit -m "feat: Add authentication feature"
git commit -m "fix: Fix null pointer exception in login module"

// Working with repositories:
// Pull requests for code review
// Code reviews and constructive feedback
// Continuous integration integration
// Automatic builds and tests triggered by commits or pull requests
```

- **DON'T**:

```text
// Bad example - Version Control

// Branching and merging strategies:
// No defined branching model
// Unstructured and arbitrary branch naming
git checkout -b my-branch
git checkout -b fix

// Commit message conventions:
// Vague and uninformative commit messages
git commit -m "Update code"
git commit -m "Fix issue"

// Working with repositories:
// Lack of pull requests and code reviews
// No integration with continuous integration
```

To effectively use version control in the project, follow these guidelines:

### Branching and merging strategies

- **Branching Model**: Follow the specified git workflow [here](./deployment#repository-branch-structure).

- **Feature Branches**: When working on new features, create a separate branch for each feature. Branch off from the main development branch and merge back when the feature is complete and tested.

- **Release Branches**: Create release branches to prepare for a new release. Perform final testing and bug fixes on the release branch before merging it into the main branch.

- **Hotfix Branches**: Create hotfix branches to address critical bugs or issues in the production environment. These branches are used for immediate fixes and are merged directly into the main branch and any active release branches.

### Commit message conventions

- **Clear and Descriptive**: Write clear and descriptive commit messages that explain the purpose of the changes. A well-crafted commit message helps others understand the context and intent of the code changes.

- **Concise and Specific**: Keep commit messages concise while providing enough information. Focus on the main changes introduced by the commit and avoid including unrelated details. Strive not just for self-documenting code, but also self-documenting commits.

- **Follow a Format**: Consider adopting a commit message format, such as the conventional commit format or a custom format agreed upon by the team. This helps maintain consistency and makes it easier to generate release notes and track changes. For this project, commitlint is used.

### Guidelines for working with repositories

- **Pull Requests**: Encourage the use of pull requests for code review and collaboration. Pull requests provide an opportunity for team members to review and provide feedback on code changes before merging them into the main branch.

- **Code Reviews**: Establish guidelines for code reviews, including expectations for reviewers and developers. Emphasize constructive feedback, adherence to coding conventions, and addressing any identified issues before merging.

- **Continuous Integration**: Integrate version control with your continuous integration system to automate build and test processes. Configure automatic builds and tests triggered by new commits or pull requests to ensure code quality and prevent integration issues.

By following these version control guidelines, you can ensure effective collaboration, maintain a clean commit history, and streamline the development process.

## Team Collaboration

For more information, see [here](./cicd.md).

- **DO**:

```text
// Good example - Team Collaboration

// Coding conventions as a collaborative tool:
// Shared understanding of coding conventions
// Consistent adherence to coding conventions
// Automated enforcement through tools like EditorConfig and StyleCop Analyzers

// Consistency and communication within the team:
// Regular communication using collaboration platforms
// Documentation and knowledge sharing
// Periodic review of coding standards and conventions

// Code review processes and best practices:
// Clear guidelines for code reviews
// Constructive feedback to improve code quality
// Utilization of code review tools for streamlined reviews
```

- **DON'T**:

```text
// Bad example - Team Collaboration

// Coding conventions as a collaborative tool:
// Lack of shared understanding and inconsistent adherence to coding conventions
// No automated enforcement of coding conventions

// Consistency and communication within the team:
// Lack of regular communication and collaboration
// Neglecting documentation and knowledge sharing
// No review of coding standards and conventions

// Code review processes and best practices:
// Absence of clear guidelines for code reviews
// Ineffective or non-constructive feedback during code reviews
// Ignoring code review tools for streamlined reviews
```

Effective team collaboration is essential for a successful software development project. Follow these guidelines to promote collaboration within the team:

### Coding conventions as a collaborative tool

- **Shared Understanding**: Coding conventions serve as a common language for the team. They promote a shared understanding of code structure, formatting, and style. Document and share the coding conventions to ensure everyone is on the same page.

- **Consistency**: Consistent coding conventions make the codebase more readable and maintainable. Encourage team members to follow the conventions consistently to improve code quality and reduce confusion.

- **Enforced Through Automation**: Leverage tools like EditorConfig and StyleCop Analyzers to automate the enforcement of coding conventions. This helps maintain consistency and reduces the need for manual code reviews.

### Consistency and communication within the team

- **Regular Communication**: Foster open and regular communication within the team. Use collaboration platforms like GitHub Discussions to facilitate communication, share ideas, and address any questions or concerns.

- **Documentation and Knowledge Sharing**: Encourage team members to document their work, share knowledge, and contribute to the project's documentation. This helps new team members onboard quickly and ensures a consistent understanding of the codebase.

- **Coding Standards Review**: Conduct periodic reviews of the coding conventions and standards to ensure they remain relevant and effective. Solicit feedback from the team and incorporate any necessary updates or improvements.

### Code review processes and best practices

- **Code Review Guidelines**: Establish clear guidelines for code reviews, including expectations for reviewers and developers. Define criteria for evaluating code quality, adherence to coding conventions, and best practices.

- **Constructive Feedback**: Encourage constructive feedback during code reviews. Focus on improving code quality, identifying potential issues, and suggesting alternative approaches. Create a positive and collaborative environment where team members can learn from each other.

- **Code Review Tools**: Utilize code review tools such as GitHub Pull Requests to streamline the review process. These tools provide features like inline comments, discussions, and automatic notifications, making it easier for team members to review and discuss code changes.

By promoting collaboration through coding conventions, maintaining consistent communication within the team, and establishing effective code review processes, we can create a productive and collaborative environment for the software development team.

## SonarCloud Analysis

The project utilizes SonarCloud for static code analysis, which helps identify code smells, bugs, and security hotspots. It is important to pay attention to SonarCloud analysis results and address any flagged issues promptly. Developers should review and resolve code smells, bugs, and security hotspots to ensure code quality and adherence to best practices. This includes refactoring code, fixing issues, and following the recommendations provided by SonarCloud. Regular analysis and resolution of SonarCloud findings contribute to a clean, maintainable, and secure codebase.

## Automatic Code Formatter

See [here](https://github.com/dotnet/format).

## Conclusion

In conclusion, coding conventions play a crucial role in software development. They provide guidelines and standards for writing clean, readable, and maintainable code. By following coding conventions, developers can:

- Improve code quality and readability.
- Enhance collaboration within the development team.
- Facilitate code reviews and reduce inconsistencies.
- Ease code maintenance and refactoring efforts.
- Promote a consistent coding style across the project.

Adhering to coding conventions not only benefits individual developers but also contributes to the overall success of the project. It fosters a more efficient and productive development process while making it easier for new team members to understand and contribute to the codebase.

As a developer, it is important to embrace coding conventions and incorporate them into your daily coding practices. Remember to utilize the available tools such as EditorConfig and StyleCop Analyzers to automate the enforcement of conventions. Additionally, keep an eye on SonarCloud for code smells, bugs, and security hotspots. Aim for a high code coverage, ideally 90% or more, and utilize code review processes to ensure code quality and consistency.

By following the guidelines outlined in this document, we can contribute to the creation of well-structured, maintainable, and high-quality code. Let's strive to write code that is not only functional but also readable, understandable, and enjoyable to work with. Together, we can build exceptional software that meets the needs of our users and stands the test of time.

**Happy ~~hunting~~ coding!**
