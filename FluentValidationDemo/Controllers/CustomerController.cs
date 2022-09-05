using FluentValidation;
using FluentValidationDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace FluentValidationDemo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private static readonly List<Customer> _customers = new();

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_customers);
    }
    [HttpPost("create-customer")]
    public IActionResult Create(Customer customer)
    {
        if (!ModelState.IsValid)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }
        _customers.Add(customer);
        return StatusCode(StatusCodes.Status200OK, "Model is valid!");
    }
    [HttpPut("update-customer/{id}")]
    public IActionResult Update(int id, Customer custome, [FromServices] IValidator<Customer> customerValidator)
    {
        var validatorResult = customerValidator.Validate(custome);
        if (!validatorResult.IsValid)
        {
            return StatusCode(StatusCodes.Status400BadRequest, validatorResult.Errors);
        }
        var customer = _customers.FirstOrDefault(x => x.Id == id);

        if (customer is null) return NotFound();

        customer.FirstName = custome.FirstName;
        customer.LastName = custome.LastName;
        customer.Address = custome.Address;
        customer.Age = custome.Age;
        customer.Phone = custome.Phone;


        return StatusCode(StatusCodes.Status200OK, "Model is valid for update!");
    }
}

