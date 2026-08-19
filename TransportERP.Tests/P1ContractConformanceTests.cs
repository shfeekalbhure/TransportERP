using System.Globalization;
using Xunit;

namespace TransportERP.Tests;

public sealed class P1ContractConformanceTests
{
    private static string DataPath(string fileName)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "P1Contracts", fileName);
        Assert.True(File.Exists(path), $"Contract evidence file is missing: {path}");
        return path;
    }

    private static List<Dictionary<string, string>> ReadCsv(string fileName)
    {
        var lines = File.ReadAllLines(DataPath(fileName));
        Assert.NotEmpty(lines);
        var headers = SplitCsv(lines[0]);
        return lines.Skip(1)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line =>
            {
                var values = SplitCsv(line);
                Assert.Equal(headers.Count, values.Count);
                return headers.Zip(values, (header, value) => new { header, value })
                    .ToDictionary(x => x.header.Trim('\uFEFF'), x => x.value);
            })
            .ToList();
    }

    private static List<string> SplitCsv(string line)
    {
        var result = new List<string>();
        var current = new System.Text.StringBuilder();
        var quoted = false;
        for (var i = 0; i < line.Length; i++)
        {
            var c = line[i];
            if (c == '"')
            {
                if (quoted && i + 1 < line.Length && line[i + 1] == '"')
                {
                    current.Append('"');
                    i++;
                }
                else
                {
                    quoted = !quoted;
                }
            }
            else if (c == ',' && !quoted)
            {
                result.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }
        result.Add(current.ToString());
        return result;
    }

    [Fact]
    public void W1_contracts_have_required_data_and_scope_fields()
    {
        var rows = ReadCsv("W1_DATA_CONTRACT_REGISTER.csv");
        Assert.Equal(17, rows.Count);
        var required = new[] { "W1_Contract_ID", "Columns_Spec", "Primary_Key", "Foreign_Keys", "Indexes_Unique", "Tenant_Company_Branch_Scope", "Concurrency", "Audit", "Lifecycle", "Migration" };
        foreach (var row in rows)
        {
            foreach (var field in required)
                Assert.False(string.IsNullOrWhiteSpace(row[field]), $"{row["W1_Contract_ID"]}: {field} is empty");
        }
        Assert.Equal(17, rows.Select(x => x["W1_Contract_ID"]).Distinct().Count());
    }

    [Fact]
    public void W2_actions_have_routes_state_and_offline_policy()
    {
        var rows = ReadCsv("W2_ACTION_CONTRACT_REGISTER.csv");
        Assert.Equal(15, rows.Count);
        var required = new[] { "Action_ID", "HTTP_Verb", "Route", "Request_DTO", "Response_DTO", "Required_Permission", "Scope", "State_Preconditions", "State_Transition", "Error_Codes", "Idempotency", "Concurrency", "Audit", "Offline_Policy", "W1_Contract_ID" };
        foreach (var row in rows)
        {
            foreach (var field in required)
                Assert.False(string.IsNullOrWhiteSpace(row[field]), $"{row["Action_ID"]}: {field} is empty");
        }
        Assert.Equal(15, rows.Select(x => x["Action_ID"]).Distinct().Count());
    }

    [Fact]
    public void W3_screens_have_contract_links_and_visual_assets()
    {
        var rows = ReadCsv("W3_SCREEN_CONTRACT_REGISTER.csv");
        Assert.Equal(12, rows.Count);
        var required = new[] { "Screen_ID", "Fields_Contract", "States", "Action_IDs", "W1_Contract_IDs", "Permissions", "Validation", "Empty_Load_Error_States", "Offline_Policy", "Audit", "Accessibility" };
        foreach (var row in rows)
        {
            foreach (var field in required)
                Assert.False(string.IsNullOrWhiteSpace(row[field]), $"{row["Screen_ID"]}: {field} is empty");
            var screenId = row["Screen_ID"];
            var asset = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "P1Screens"), $"{screenId}_*.png").SingleOrDefault();
            Assert.NotNull(asset);
            Assert.True(new FileInfo(asset!).Length > 0, $"{screenId}: image is empty");
        }
        Assert.Equal(12, rows.Select(x => x["Screen_ID"]).Distinct().Count());
    }

    [Fact]
    public void Acceptance_register_contains_all_203_specified_cases()
    {
        var rows = ReadCsv("P1_ACCEPTANCE_TEST_REGISTER.csv");
        Assert.Equal(203, rows.Count);
        Assert.All(rows, row => Assert.Equal("SPECIFIED_NOT_EXECUTED", row["Execution_Status"]));
        Assert.Equal(203, rows.Select(x => x["Test_ID"]).Distinct().Count());
    }

    [Fact]
    public void Sync_contract_has_required_conflict_and_retry_controls()
    {
        var path = DataPath("P1_SYNC_CONTRACT.md");
        var content = File.ReadAllText(path);
        foreach (var term in new[] { "ClientOperationId", "PayloadHash", "CONFLICT", "retry", "idempot", "Offline", "Online" })
            Assert.Contains(term, content, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void P1_contracts_do_not_claim_provider_specific_schema_or_migrations()
    {
        var root = Path.Combine(AppContext.BaseDirectory, "P1Contracts");
        var text = string.Join("\n", Directory.GetFiles(root, "*.csv").Select(File.ReadAllText));
        Assert.DoesNotContain("CREATE TABLE", text, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Production DB", text, StringComparison.OrdinalIgnoreCase);
    }
}
