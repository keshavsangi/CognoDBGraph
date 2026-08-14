using Neo4j.Driver;
using CognoDBGraph.Models;

namespace CognoDBGraph.Services;

public class GraphService : IAsyncDisposable
{
    private readonly IDriver _driver;

    public GraphService(IConfiguration config)
    {
        
        var uri = config["CognoDB:Uri"];
        var user = config["CognoDB:Username"];
        var password = config["CognoDB:Password"];

        _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
    }

    public async Task SeedDataAsync()
    {
        await using var session = _driver.AsyncSession();
        await session.ExecuteWriteAsync(async tx =>
        {
            await tx.RunAsync("MATCH(n) DETACH DELETE n");

            var cypher = @"
            

            CREATE (d1:Developer {id: 'dev1', name: 'MS Dhoni', role: '.NET Backend Developer'})
            CREATE (d2:Developer {id: 'dev2', name: 'Virat Kohli', role: 'Full Stack Engineer'})
            CREATE (d3:Developer {id: 'dev3', name: 'Suresh Raina', role: 'Cloud Architect'})

            CREATE (s1:Skill {name: 'C#'})
            CREATE (s2:Skill {name: 'ASP.NET Core'})
            CREATE (s3:Skill {name: 'CognoDB'})
            CREATE (s4:Skill {name: 'React'})

            CREATE (d1)-[:HAS_SKILL]->(s1)
            CREATE (d1)-[:HAS_SKILL]->(s2)
            CREATE (d1)-[:HAS_SKILL]->(s3)

            CREATE (d2)-[:HAS_SKILL]->(s1)
            CREATE (d2)-[:HAS_SKILL]->(s4)

            CREATE (d3)-[:HAS_SKILL]->(s3);
        ";

            await tx.RunAsync(cypher);
        });
    }

    public async Task<List<string>> GetSkillsAsync(string devId)
    {
        await using var session = _driver.AsyncSession();
        var cypher = @"
            MATCH (d:Developer {id: $devId})-[:HAS_SKILL]->(s:Skill)
            RETURN s.name AS skillName";

        var result = await session.ExecuteReadAsync(async tx =>
        {
            var cursor = await tx.RunAsync(cypher, new { devId });
            var records = await cursor.ToListAsync();
            return records.Select(r => r["skillName"].As<string>()).ToList();
        });

        return result;
    }

    // 2-Hop Cypher Traversal
    public async Task<List<TeammateRecommendation>> GetRecommendedTeammatesAsync(string devId)
    {
        await using var session = _driver.AsyncSession();
        var cypher = @"
            MATCH (d1:Developer {id: $devId})-[:HAS_SKILL]->(s:Skill)<-[:HAS_SKILL]-(d2:Developer)
            WHERE d1 <> d2
            RETURN d2.name AS Name, d2.role AS Role, s.name AS SharedSkill";

        var result = await session.ExecuteReadAsync(async tx =>
        {
            var cursor = await tx.RunAsync(cypher, new { devId });
            var records = await cursor.ToListAsync();
            return records.Select(r => new TeammateRecommendation
            {
                Name = r["Name"].As<string>(),
                Role = r["Role"].As<string>(),
                SharedSkill = r["SharedSkill"].As<string>()
            }).ToList();
        });

        return result;
    }

    public async ValueTask DisposeAsync()
    {
        await _driver.DisposeAsync();
    }
}