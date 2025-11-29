# Simulador de Sistema Operacional

## Descrição do Projeto

Este projeto consiste em um **simulador completo de Sistema Operacional**, desenvolvido para ensino e experimentação de conceitos clássicos de gerenciamento de processos, threads, escalonamento de CPU, memória, entrada/saída e sistema de arquivos.

O simulador é **totalmente independente do sistema operacional hospedeiro**, utilizando simulações lógicas de processos, threads e recursos de hardware. Ele é configurável via **linha de comando** ou arquivos de workload, permitindo modo determinístico para testes reproduzíveis e garantindo **reprodutibilidade de experimentos**.

O sistema gera **logs detalhados**, mantém estatísticas sobre tempo de CPU, filas de pronto, dispositivos de E/S, TLB, utilização de memória e operações de sistema de arquivos. É uma ferramenta educacional robusta para **análise do comportamento de políticas clássicas de SO**, permitindo estudos comparativos e análise de desempenho em diferentes cenários.

O simulador permite:

* Simulação de múltiplos processos e threads com alternância e preempção.
* Observação de métricas detalhadas de memória, CPU, E/S e sistema de arquivos.
* Execução de workloads configuráveis via linha de comando.
* Análise de políticas clássicas de escalonamento, alocação de memória e gerenciamento de E/S.
* Registro detalhado de eventos para estudo e depuração.

---

## Participantes

| Nome                             | RA     | E-mail                                                              |
| -------------------------------- | ------ | ------------------------------------------------------------------- |
| Guilherme Augusto Scaglia        | 111598 | [scaglia@alunos.fho.edu.br](mailto:scaglia@alunos.fho.edu.br)       |
| João Pedro Denardo               | 113036 | [denardo749@alunos.fho.edu.br](mailto:denardo749@alunos.fho.edu.br) |
| Pedro Henrique Oliveira de Souza | 113364 | [pedro1204@alunos.fho.edu.br](mailto:pedro1204@alunos.fho.edu.br)   |

---

## Funcionalidades Implementadas

### 1. Processos

* Criação, execução e gerenciamento de processos com estados clássicos: **Novo, Pronto, Executando, Bloqueado e Finalizado**.
* Cada processo possui um **PCB (Process Control Block)** contendo:

  * PID único e sequencial.
  * Registradores simulados para execução lógica.
  * Contador de programa lógico (PC simulado).
  * Prioridade inteira simples.
  * Tabela de arquivos abertos.

**Detalhes de fluxo interno**:

* Ao criar um processo, ele inicia no estado **Novo**.
* O escalonador move o processo para **Pronto** quando houver CPU disponível.
* Durante execução, se o processo requisitar E/S, ele passa para **Bloqueado** e retorna a **Pronto** após a operação.
* Ao concluir execução, o processo passa para **Finalizado**, liberando recursos.

**Exemplo de uso**: criar um processo, adicionar threads e monitorar mudanças de estado via log detalhado.

---

### 2. Threads

* Threads de usuário vinculadas a processos.
* Estados equivalentes aos de processos.
* Cada thread possui **TCB (Thread Control Block)**:

  * Identificador único.
  * Referência ao processo pai.
  * Pilha lógica simulada.
  * Política de escalonamento compatível com o processo.

**Fluxo interno**:

* Threads competem pela CPU junto com outras threads do mesmo ou de outros processos.
* Preempção e alternância simuladas permitem analisar comportamento de sistemas multitarefa.
* Logs de eventos detalham transições de estado, trocas de contexto e tempo de CPU consumido por thread.

---

### 3. Escalonador de CPU

* Algoritmos configuráveis:

  * **FCFS (First-Come, First-Served)**
  * **Round-Robin** com quantum configurável
  * **Prioridades** (preemptivo e não preemptivo)
* Simulação de troca de contexto:

  * Contagem de trocas
  * Sobrecarga de tempo configurável
* Fila de prontos mantém threads e processos, permitindo análise de métricas como:

  * Tempo médio de espera
  * Tempo de resposta
  * Turnaround
  * Throughput

**Experimentos possíveis**: comparar FCFS e Round-Robin para workloads com diferentes números de threads e processos, analisando tempo de resposta e eficiência de CPU.

---

### 4. Gerenciamento de Memória

* Paginação simples com tamanho de página fixo.
* Política **First Fit** para alocação de molduras.
* **TLB lógica** com contagem de hits e misses.
* Métricas básicas: páginas alocadas, molduras livres, hits/misses de TLB.
* Alternativa de segmentação simples aceita, mas não implementada.

**Detalhes avançados**:

* Cada processo possui espaço de endereçamento lógico isolado.
* Falta de página gera interrupção simulada, contabilizando estatísticas.
* Logs registram acessos à memória, falhas de página e hits/misses de TLB.

---

### 5. Entrada e Saída (E/S)

* Dispositivos simulados de **bloco e caractere**.
* Filas de pedidos por dispositivo, tempos de serviço configuráveis.
* Geração de interrupções e API de E/S:

  * Bloqueante
  * Não bloqueante
  * Sem espera ativa
* Permite simular concorrência de processos aguardando dispositivos diferentes.

**Exemplos educacionais**:

* Medir tempo médio de espera de processos na fila de E/S.
* Avaliar efeito de múltiplos dispositivos e tempos de serviço na eficiência do sistema.

---

### 6. Sistema de Arquivos

* Estrutura hierárquica com diretório raiz, subdiretórios e arquivos.
* Operações implementadas:

  * Criar, abrir, ler, escrever, fechar, apagar, listar arquivos.
* Tabelas de arquivos globais e por processo.
* Cache de blocos opcional ainda não implementado.
* Simulação de concorrência entre processos para acesso a arquivos.

**Exemplos de análise**:

* Medir tempo médio de acesso a arquivos.
* Avaliar impacto de múltiplos processos acessando o mesmo arquivo.

---

### 7. Métricas e Registro

* Logs detalhados de eventos e clock.
* Estatísticas básicas:

  * Tempo de retorno por processo
  * Tempo de espera em fila de pronto
  * Tempo de resposta da primeira CPU
  * Número de trocas de contexto e sobrecarga de escalonamento
* Métricas avançadas parcialmente implementadas:

  * Throughput
  * Utilização de CPU e dispositivos
  * Taxa de falta de página

**Uso didático**: comparar métricas entre políticas de escalonamento e tamanhos de memória diferentes, permitindo análise quantitativa de desempenho.

---

### 8. Interface de Uso

* Linha de comando configurável:

  * Algoritmo de escalonamento
  * Quantum
  * Tamanho de página
  * Número de molduras
  * Tempos de dispositivos
  * Caminho para workload
* Modo determinístico com semente fixa
* Interface gráfica não implementada, mas logs e visualizações via terminal permitem acompanhamento detalhado da execução.

---

## Estrutura do Projeto

* `Nucleo` – inicialização e gerenciamento central do simulador; Geração de logs e estatísticas; Simulação de dispositivos; Algoritmos de CPU configuráveis
* `Processos_PassouAqui` – controle de PCBs; Paginação, molduras, TLB e estatísticas básicas; Controle de TCBs; 
* `SistemaDeArquivos` – operações de arquivos e diretórios;  Funções de apoio, IDs e aleatoriedade
* `Interface` – execução de workloads e linha de comando.
---

## Progresso do Projeto

* O simulador **atingiu aproximadamente 80-85% do escopo mínimo**.
* Funcionalidades sólidas:

  * Processos, threads, escalonamento, memória básica, E/S e sistema de arquivos
* Funcionalidades incompletas ou parciais:

  * Estatísticas detalhadas de TLB, taxa de falta de página e throughput
  * Cache de blocos no sistema de arquivos
  * Políticas de alocação de memória Best Fit
  * Métricas detalhadas de utilização de dispositivos e tempo de retorno

![Progresso do Simulador](https://media.giphy.com/media/3o7TKPj6q0hWcdN0Vm/giphy.gif)

---

## Limitações Conhecidas

* Não há suporte a multiprocessamento real.
* Não há suporte a drivers reais do sistema hospedeiro.
* Threads reais da linguagem não interagem diretamente com a simulação.
* Algumas métricas avançadas de performance ainda não implementadas.

---

## Observações Finais

O simulador é **funcional para experimentos e testes educacionais**, permitindo analisar o comportamento de processos, threads, escalonamento e memória em ambiente controlado. Funcionalidades avançadas de monitoramento e cache podem ser implementadas em versões futuras para aprimorar métricas, performance e análise comparativa de políticas clássicas de SO.

---

## Exemplos de Experimentos Educacionais

1. Comparar tempos de espera e turnaround entre FCFS e Round-Robin.
2. Avaliar efeito do tamanho da memória na taxa de faltas de página.
3. Medir utilização de CPU e dispositivos sob diferentes workloads.
4. Analisar impacto da quantidade de threads por processo no tempo de resposta.
5. Avaliar concorrência de múltiplos processos acessando arquivos e dispositivos de E/S.
6. Simular cenários com cargas de trabalho variadas para análise de preempção e bloqueio.
7. Testar efeitos de quantum diferente em Round-Robin sobre throughput e latência.

---

## Referências

* Material de disciplinas de Engenharia da Computação da FHO.
* TANENBAUM, Andrew S.; BOS, Herbert. Sistemas Operacionais Modernos. 4. ed. São Paulo: Pearson, 2015.

---
## Orientador e Agradecimentos

Orientador: Prof. Dr. Mauricio Acconcia Dias

Agradecemos ao Prof. Mauricio Acconcia Dias pela orientação, suporte e valiosas contribuições durante o desenvolvimento deste projeto, que foram fundamentais para o sucesso do simulador.
