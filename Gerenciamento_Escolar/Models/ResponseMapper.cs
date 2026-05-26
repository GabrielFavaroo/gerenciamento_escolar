using System.Collections;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Gerenciamento_Escolar.Models;

public class ResponseMapper
{
    
    public static IActionResult createHttpResponse<T>(Result<T> result,ControllerBase controller)
    {

        return result.statusCode switch
        {
            200 => controller.Ok(result),
            201 => controller.Created(),
            204 => controller.NoContent(),
            404 => controller.NotFound(result),
            401 => controller.Unauthorized(result),
            409 => controller.Conflict(result),
            403 => controller.Forbid(),
            422 => controller.UnprocessableEntity(result),

            _ => controller.StatusCode(500, new { message = "Ocorreu um problema inesperado" })
        };

    }
}