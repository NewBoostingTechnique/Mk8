using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mk8.Core.ProofTypes;
using Mk8.Web.App;

namespace Mk8.Web.ProofTypes;

[Route("api/proofType")]
public class ProofTypeApi(IProofTypeService proofService) : Api
{
    [AllowAnonymous, HttpGet("")]
    public async Task<IActionResult> ListAsync()
    {
        return Ok(await proofService.ListAsync());
    }
}