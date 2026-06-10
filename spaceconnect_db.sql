-- ============================================================
--  SpaceConnect Database
--  Nova Economia Espacial - FIAP GS 2025
-- ============================================================

CREATE DATABASE IF NOT EXISTS spaceconnect
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE spaceconnect;

-- ------------------------------------------------------------
--  Categorias de Impacto
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS categorias_impacto (
    id          INT AUTO_INCREMENT PRIMARY KEY,
    nome        VARCHAR(100) NOT NULL,
    descricao   TEXT,
    icone       VARCHAR(50),
    cor_hex     VARCHAR(7) DEFAULT '#ef4444',
    criado_em   DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- ------------------------------------------------------------
--  Missoes Espaciais
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS missoes (
    id          INT AUTO_INCREMENT PRIMARY KEY,
    nome        VARCHAR(150) NOT NULL,
    agencia     VARCHAR(100) NOT NULL,
    ano         INT NOT NULL,
    descricao   TEXT,
    criado_em   DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- ------------------------------------------------------------
--  Usuarios
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS usuarios (
    id              INT AUTO_INCREMENT PRIMARY KEY,
    nome            VARCHAR(150) NOT NULL,
    email           VARCHAR(200) NOT NULL UNIQUE,
    senha_hash      VARCHAR(255) NOT NULL,
    perfil          ENUM('Pesquisador', 'Administrador') DEFAULT 'Pesquisador',
    ativo           TINYINT(1) DEFAULT 1,
    criado_em       DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- ------------------------------------------------------------
--  Tecnologias
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS tecnologias (
    id              INT AUTO_INCREMENT PRIMARY KEY,
    nome            VARCHAR(200) NOT NULL,
    descricao       TEXT NOT NULL,
    ano_origem      INT,
    aplicacao_terra TEXT,
    categoria_id    INT NOT NULL,
    missao_id       INT,
    cadastrado_por  INT,
    criado_em       DATETIME DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_tech_categoria FOREIGN KEY (categoria_id)
        REFERENCES categorias_impacto(id) ON DELETE RESTRICT,
    CONSTRAINT fk_tech_missao FOREIGN KEY (missao_id)
        REFERENCES missoes(id) ON DELETE SET NULL,
    CONSTRAINT fk_tech_usuario FOREIGN KEY (cadastrado_por)
        REFERENCES usuarios(id) ON DELETE SET NULL
);

-- ------------------------------------------------------------
--  Dados iniciais - Categorias
-- ------------------------------------------------------------
INSERT INTO categorias_impacto (nome, descricao, icone, cor_hex) VALUES
('Saúde',           'Tecnologias aplicadas à medicina e bem-estar humano',            'heart',        '#ef4444'),
('Agricultura',     'Inovações para produção de alimentos e manejo do solo',           'leaf',         '#22c55e'),
('Consumo',         'Produtos e materiais presentes no cotidiano',                     'shopping-bag', '#3b82f6'),
('Comunicação',     'Sistemas de telecomunicação e conectividade global',              'radio',        '#f97316'),
('Energia',         'Fontes e soluções para geração e armazenamento de energia',       'zap',          '#eab308'),
('Segurança',       'Equipamentos e sistemas para proteção humana e de estruturas',    'shield',       '#8b5cf6'),
('Transporte',      'Inovações em mobilidade terrestre, aérea e espacial',             'rocket',       '#06b6d4'),
('Meio Ambiente',   'Monitoramento climático e preservação ambiental',                 'globe',        '#10b981');

-- ------------------------------------------------------------
--  Dados iniciais - Missoes
-- ------------------------------------------------------------
INSERT INTO missoes (nome, agencia, ano, descricao) VALUES
('Apollo 11',           'NASA',   1969, 'Primeira missão a pousar humanos na Lua'),
('Apollo 17',           'NASA',   1972, 'Última missão Apollo com astronautas na Lua'),
('Skylab',              'NASA',   1973, 'Primeira estação espacial americana'),
('Space Shuttle',       'NASA',   1981, 'Programa de ônibus espaciais reutilizáveis'),
('ISS - Estação Espacial Internacional', 'NASA / Roscosmos', 1998, 'Laboratório orbital permanente'),
('Hubble Space Telescope', 'NASA', 1990, 'Telescópio orbital de alta resolução'),
('Mars Curiosity',      'NASA',   2012, 'Rover de exploração da superfície de Marte'),
('SpaceX Crew Dragon',  'SpaceX', 2020, 'Primeira cápsula comercial a levar humanos à ISS');

-- ------------------------------------------------------------
--  Dados iniciais - Usuarios (BCrypt)
--  admin@spaceconnect.fiap / Admin@2025
--  pesquisador@spaceconnect.fiap / Pesq@2025
-- ------------------------------------------------------------
INSERT INTO usuarios (nome, email, senha_hash, perfil, ativo) VALUES
('Administrador Demo', 'admin@spaceconnect.fiap', '$2a$11$MWL9vTeCw1Wz2q6c2caAAe5c6lbYSXp00e6sF0qWPvK8McDA3JgHO', 'Administrador', 1),
('Pesquisador Demo', 'pesquisador@spaceconnect.fiap', '$2a$11$e50MSvd7PxsOKslifK5nLuvMEqD6xqfR8PshM94qlPoAp5naeBLj2', 'Pesquisador', 1);

-- ------------------------------------------------------------
--  Dados iniciais - Tecnologias
-- ------------------------------------------------------------
INSERT INTO tecnologias (nome, descricao, ano_origem, aplicacao_terra, categoria_id, missao_id) VALUES
(
    'Espuma Viscoelástica',
    'Material desenvolvido pela NASA para absorver impacto em assentos de aeronaves e cápsulas espaciais. A estrutura de célula aberta distribui pressão de forma uniforme.',
    1966,
    'Colchões, capacetes, palmilhas ortopédicas e assentos de automóveis',
    3,
    1
),
(
    'Sensores de Imagem CMOS',
    'Tecnologia de captura de imagem miniaturizada criada para missões espaciais onde peso e energia são críticos.',
    1990,
    'Câmeras de smartphone, câmeras de segurança, scanners médicos',
    3,
    6
),
(
    'Filtros de Purificação de Água',
    'Sistema de purificação por iodo desenvolvido para garantir água potável em longa duração fora da Terra.',
    1972,
    'Filtros domésticos, tratamento de água em zonas de desastre',
    1,
    2
),
(
    'Monitoramento Cardíaco Wireless',
    'Sensores de telemetria biométrica criados para monitorar astronautas à distância durante missões.',
    1969,
    'Smartwatches, monitores cardíacos hospitalares sem fio',
    1,
    1
),
(
    'Comunicação por Satélite',
    'Rede de retransmissão de sinais para manter contato com missões além da órbita baixa.',
    1965,
    'Internet global, GPS, televisão a cabo, telefonia móvel',
    4,
    3
),
(
    'Painel Solar de Alta Eficiência',
    'Células fotovoltaicas com conversão de energia otimizada para alimentar equipamentos em órbita sem reabastecimento.',
    1973,
    'Energia solar residencial, carregadores portáteis, sistemas off-grid',
    5,
    3
),
(
    'Ferramentas Sem Fio',
    'Ferramentas de torque com bateria desenvolvidas para uso extravehicular em ambiente de microgravidade.',
    1961,
    'Furadeiras, parafusadeiras e ferramentas elétricas portáteis',
    3,
    1
),
(
    'Isolamento Térmico Multicamada',
    'Material de isolamento refletivo com múltiplas camadas de mylar e dacron para extremos térmicos do espaço.',
    1964,
    'Cobertores de emergência, roupas de alto desempenho, construção civil',
    6,
    1
),
(
    'Navegação GPS',
    'Sistema de posicionamento global baseado em satélites para orientação precisa de naves e veículos.',
    1978,
    'Navegação veicular, logística, agricultura de precisão',
    7,
    4
),
(
    'Liofilização de Alimentos',
    'Técnica de desidratação a vácuo para preservar alimentos por longos períodos sem refrigeração.',
    1965,
    'Comida de camping, rações militares, produtos alimentícios industrializados',
    2,
    3
);

-- ------------------------------------------------------------
--  Indice para consultas frequentes
-- ------------------------------------------------------------
CREATE INDEX idx_tech_categoria ON tecnologias(categoria_id);
CREATE INDEX idx_tech_missao    ON tecnologias(missao_id);
CREATE INDEX idx_tech_criado    ON tecnologias(criado_em);
