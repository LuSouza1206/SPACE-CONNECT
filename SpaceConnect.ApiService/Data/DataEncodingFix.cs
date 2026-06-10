using Microsoft.EntityFrameworkCore;

namespace SpaceConnect.ApiService.Data;

public static class DataEncodingFix
{
    private static bool HasMojibake(string? text) =>
        text != null && text.Contains('\u251c');

    public static async Task ApplyAsync(SpaceConnectContext db)
    {
        var techSample = await db.Tecnologias.AsNoTracking().Select(t => t.Nome).FirstOrDefaultAsync();
        if (techSample != null && HasMojibake(techSample))
            await FixCategoriasAndTecnologiasAsync(db);

        var missoes = await db.Missoes.AsNoTracking()
            .Select(m => new { m.Nome, m.Descricao })
            .ToListAsync();
        if (missoes.Any(m => HasMojibake(m.Nome) || HasMojibake(m.Descricao)))
            await FixMissoesAsync(db);
    }

    private static async Task FixCategoriasAndTecnologiasAsync(SpaceConnectContext db)
    {
        var categorias = new Dictionary<int, (string Nome, string Descricao)>
        {
            [1] = ("Saúde", "Tecnologias aplicadas à medicina e bem-estar humano"),
            [2] = ("Agricultura", "Inovações para produção de alimentos e manejo do solo"),
            [3] = ("Consumo", "Produtos e materiais presentes no cotidiano"),
            [4] = ("Comunicação", "Sistemas de telecomunicação e conectividade global"),
            [5] = ("Energia", "Fontes e soluções para geração e armazenamento de energia"),
            [6] = ("Segurança", "Equipamentos e sistemas para proteção humana e de estruturas"),
            [7] = ("Transporte", "Inovações em mobilidade terrestre, aérea e espacial"),
            [8] = ("Meio Ambiente", "Monitoramento climático e preservação ambiental")
        };

        foreach (var (id, data) in categorias)
        {
            await db.Categorias.Where(c => c.Id == id).ExecuteUpdateAsync(s => s
                .SetProperty(c => c.Nome, data.Nome)
                .SetProperty(c => c.Descricao, data.Descricao));
        }

        var tecnologias = new Dictionary<int, (string Nome, string Descricao, string Aplicacao)>
        {
            [1] = (
                "Espuma Viscoelástica",
                "Material desenvolvido pela NASA para absorver impacto em assentos de aeronaves e cápsulas espaciais. A estrutura de célula aberta distribui pressão de forma uniforme.",
                "Colchões, capacetes, palmilhas ortopédicas e assentos de automóveis"
            ),
            [2] = (
                "Sensores de Imagem CMOS",
                "Tecnologia de captura de imagem miniaturizada criada para missões espaciais onde peso e energia são críticos.",
                "Câmeras de smartphone, câmeras de segurança, scanners médicos"
            ),
            [3] = (
                "Filtros de Purificação de Água",
                "Sistema de purificação por iodo desenvolvido para garantir água potável em longa duração fora da Terra.",
                "Filtros domésticos, tratamento de água em zonas de desastre"
            ),
            [4] = (
                "Monitoramento Cardíaco Wireless",
                "Sensores de telemetria biométrica criados para monitorar astronautas à distância durante missões.",
                "Smartwatches, monitores cardíacos hospitalares sem fio"
            ),
            [5] = (
                "Comunicação por Satélite",
                "Rede de retransmissão de sinais para manter contato com missões além da órbita baixa.",
                "Internet global, GPS, televisão a cabo, telefonia móvel"
            ),
            [6] = (
                "Painel Solar de Alta Eficiência",
                "Células fotovoltaicas com conversão de energia otimizada para alimentar equipamentos em órbita sem reabastecimento.",
                "Energia solar residencial, carregadores portáteis, sistemas off-grid"
            ),
            [7] = (
                "Ferramentas Sem Fio",
                "Ferramentas de torque com bateria desenvolvidas para uso extravehicular em ambiente de microgravidade.",
                "Furadeiras, parafusadeiras e ferramentas elétricas portáteis"
            ),
            [8] = (
                "Isolamento Térmico Multicamada",
                "Material de isolamento refletivo com múltiplas camadas de mylar e dacron para extremos térmicos do espaço.",
                "Cobertores de emergência, roupas de alto desempenho, construção civil"
            ),
            [9] = (
                "Navegação GPS",
                "Sistema de posicionamento global baseado em satélites para orientação precisa de naves e veículos.",
                "Navegação veicular, logística, agricultura de precisão"
            ),
            [10] = (
                "Liofilização de Alimentos",
                "Técnica de desidratação a vácuo para preservar alimentos por longos períodos sem refrigeração.",
                "Comida de camping, rações militares, produtos alimentícios industrializados"
            )
        };

        foreach (var (id, data) in tecnologias)
        {
            await db.Tecnologias.Where(t => t.Id == id).ExecuteUpdateAsync(s => s
                .SetProperty(t => t.Nome, data.Nome)
                .SetProperty(t => t.Descricao, data.Descricao)
                .SetProperty(t => t.AplicacaoTerra, data.Aplicacao));
        }
    }

    private static async Task FixMissoesAsync(SpaceConnectContext db)
    {
        var missoes = new Dictionary<int, (string Nome, string Descricao)>
        {
            [1] = ("Apollo 11", "Primeira missão a pousar humanos na Lua"),
            [2] = ("Apollo 17", "Última missão Apollo com astronautas na Lua"),
            [3] = ("Skylab", "Primeira estação espacial americana"),
            [4] = ("Space Shuttle", "Programa de ônibus espaciais reutilizáveis"),
            [5] = ("ISS - Estação Espacial Internacional", "Laboratório orbital permanente"),
            [6] = ("Hubble Space Telescope", "Telescópio orbital de alta resolução"),
            [7] = ("Mars Curiosity", "Rover de exploração da superfície de Marte"),
            [8] = ("SpaceX Crew Dragon", "Primeira cápsula comercial a levar humanos à ISS")
        };

        foreach (var (id, data) in missoes)
        {
            await db.Missoes.Where(m => m.Id == id).ExecuteUpdateAsync(s => s
                .SetProperty(m => m.Nome, data.Nome)
                .SetProperty(m => m.Descricao, data.Descricao));
        }
    }
}
