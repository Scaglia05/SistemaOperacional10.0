# Simulador de Sistema Operacional

## Descrição do Projeto

Este projeto consiste em um **simulador completo de Sistema Operacional**, desenvolvido com o objetivo de ensinar e experimentar conceitos clássicos de gerenciamento de processos, threads, escalonamento de CPU, memória, entrada/saída e sistema de arquivos.

O simulador é **totalmente independente do sistema operacional hospedeiro**, utilizando simulações lógicas de processos, threads e recursos de hardware. É configurável via **linha de comando** ou arquivos de workload, permitindo modo determinístico para testes reproduzíveis.

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

* Criação e gerenciamento de processos com estados clássicos:

  * **Novo, Pronto, Executando, Bloqueado e Finalizado**.
* Cada processo possui um **PCB (Process Control Block)** com:

  * PID único.
  * Registradores simulados.
  * Contador de programa lógico.
  * Prioridade simples.
  * Tabela de arquivos abertos.

### 2. Threads

* Threads de usuário vinculadas a processos.
* Estados de threads equivalentes aos de processo.
* Cada thread possui um **TCB (Thread Control Block)** com:

  * Identificador único.
  * Referência ao processo pai.
  * Pilha lógica simulada.
  * Política de escalonamento compatível com o processo.

### 3. Escalonador de CPU

* Algoritmos configuráveis:

  * **FCFS (First-Come, First-Served)**.
  * **Round-Robin** com quantum configurável.
  * **Prioridades** (preemptivo e não preemptivo).
* Simulação de troca de contexto, contagem de trocas e sobrecarga de tempo configurável.

### 4. Gerenciamento de Memória

* Paginação simples com tamanho de página fixo.
* Política **First Fit** para alocação de molduras.
* TLB lógica implementada, com contagem de hits e misses.
* Alternativa de segmentação não implementada.
* Métricas detalhadas de taxa de falta de página e throughput parcialmente implementadas.

### 5. Entrada e Saída (E/S)

* Simulação de dispositivos genéricos (bloco e caractere).
* Filas de pedidos por dispositivo e tempos de serviço simulados.
* API de E/S bloqueante e não bloqueante, sem espera ativa.

### 6. Sistema de Arquivos

* Estrutura hierárquica com diretório raiz, subdiretórios e arquivos.
* Operações implementadas:

  * Criar, abrir, ler, escrever, fechar, apagar e listar arquivos.
* Tabelas de arquivos globais e por processo.
* Cache de blocos opcional ainda não implementado.

### 7. Métricas e Registro

* Logs de eventos e clock do simulador.
* Estatísticas básicas geradas ao final da simulação.
* Métricas avançadas como throughput detalhado, tempo de retorno e utilização de dispositivos parcialmente implementadas.

### 8. Interface de Uso

* Linha de comando para configuração de:

  * Algoritmo de escalonamento.
  * Quantum.
  * Tamanho de página.
  * Número de molduras.
  * Tempos de dispositivos.
  * Caminho para workload.
* Modo determinístico com semente fixa.
* Interface gráfica **não implementada**, mas não obrigatória.

---

## Estrutura do Projeto

* `Nucleo_PassouAqui` – inicialização e gerenciamento central do simulador.
* `Processos_PassouAqui` – controle de PCBs.
* `Threads_PassouAqui` – controle de TCBs.
* `Escalonador_PassouAqui` – algoritmos de CPU configuráveis.
* `Memoria_PassouAqui` – paginação, molduras, TLB e estatísticas básicas.
* `EntradaSaida_PassouAqui` – simulação de dispositivos.
* `SistemaDeArquivos_PassouAqui` – operações de arquivos e diretórios.
* `Interface_PassouAqui` – execução de workloads e linha de comando.
* `Metricas_PassouAqui` – geração de logs e estatísticas.
* `Utilitarios` – funções de apoio, IDs e aleatoriedade.

---

## Progresso do Projeto

* O simulador **atingiu aproximadamente 80-85% do escopo mínimo**.
* **Funcionalidades sólidas:** Processos, threads, escalonamento, memória básica, operações de E/S e sistema de arquivos.
* **Funcionalidades incompletas ou parciais:**

  * Estatísticas detalhadas de TLB, taxa de falta de página e throughput.
  * Cache de blocos no sistema de arquivos.
  * Políticas de alocação de memória Best Fit.
  * Métricas detalhadas de utilização de dispositivos e tempo de retorno.

---

## Limitações Conhecidas

* Não há suporte a multiprocessamento real.
* Não há suporte a drivers reais do sistema hospedeiro.
* Threads reais da linguagem não interagem diretamente com a simulação.
* Algumas métricas avançadas de performance ainda não estão implementadas.

---

## Observações Finais

O simulador é **funcional para experimentos e testes educacionais**, permitindo analisar o comportamento de processos, threads, escalonamento e memória em um ambiente controlado. Funcionalidades avançadas de monitoramento e cache podem ser implementadas em futuras versões para aprimorar métricas e performance.
