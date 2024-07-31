using Microsoft.AspNetCore.Mvc;
using PubSubServer.Dto;
using PubSubServer.Node;

namespace PubSubServer.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class NodeController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var nodes = NodeManager.Get();
        return Ok(nodes.Select(MapNodeItemToNodeDto));
    }
    [HttpGet]
    [Route("{name}")]
    public IActionResult Get(string name)
    {
        var node = NodeManager.Get(name);
        if (node == null)
            return NotFound();
        return Ok(MapNodeItemToNodeDto(node));
    }
    
    private NodeDto MapNodeItemToNodeDto(NodeItem nodeItem)
    {
        return new NodeDto()
        {
            Name = nodeItem.Name,
            Value = nodeItem.Value,
            LastUpdated = nodeItem.LastUpdated
        };
    }
}