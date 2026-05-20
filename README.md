# Corre, Calouro! 🎮☕

## Sobre o projeto

**Corre, Calouro!** é um jogo 2D desenvolvido em Unity para a disciplina de **Computação Gráfica e Desenvolvimento de Jogos / CIC402 – Projeto T1**.

O jogo tem como tema a rotina de um estudante atrasado para a aula. O jogador controla um aluno que corre pelo campus da faculdade e precisa desviar de obstáculos, pular, coletar cafés e chegar a tempo na sala.

A proposta foi inspirada em jogos de corrida infinita, como **Subway Surfers**, mas adaptada para um ambiente universitário e desenvolvida em 2D.

---

## Objetivo do jogo

O objetivo do jogador é ajudar o calouro a chegar a tempo na aula, desviando dos obstáculos espalhados pelo campus e coletando cafés para aumentar sua pontuação.

Durante o caminho, o jogador encontra objetos comuns do ambiente universitário, como mochilas, catracas, carteiras, livros e outros elementos que dificultam sua chegada.

---

## Mecânicas principais

O jogo possui as seguintes mecânicas:

- Corrida automática do personagem;
- Movimentação entre faixas;
- Pulo para desviar de obstáculos;
- Coleta de cafés;
- Sistema de pontuação;
- Contador de cafés coletados;
- Fases com dificuldade progressiva;
- Tela de Game Over;
- Tela de vitória;
- Primeira fase em formato de tutorial.

---

## Controles

| Tecla | Ação |
|------|------|
| A ou seta esquerda | Move o personagem para a esquerda |
| D ou seta direita | Move o personagem para a direita |
| Espaço ou seta para cima | Faz o personagem pular |

---

## Estrutura das fases

### Fase 1 — Tutorial

A primeira fase funciona como uma fase demonstrativa. Nela, o jogador aprende os comandos principais do jogo enquanto joga.

O tutorial ensina:

- Como mover para a esquerda;
- Como mover para a direita;
- Como pular;
- Como coletar cafés;
- Como evitar obstáculos.

Essa fase foi criada para atender ao requisito da disciplina de que a primeira fase funcione como um tutorial para o restante do jogo.

### Fase 2 — Campus

A segunda fase representa a corrida pelo campus da faculdade.

Nessa fase, o jogador já precisa desviar dos obstáculos e coletar cafés sem instruções guiadas. A dificuldade é intermediária, com obstáculos mais frequentes e maior necessidade de reação.

### Fase 3 — Chegada na aula

A terceira fase representa a parte final do percurso, próxima à sala de aula.

Ela possui maior dificuldade, com mais obstáculos, velocidade mais alta e menor tempo de reação. Ao concluir essa fase, o jogador vence o jogo.

---

## Pontuação

O jogador ganha pontos ao coletar cafés durante as fases.

A HUD do jogo exibe:

- Pontuação atual;
- Quantidade de cafés coletados;
- Nome da fase atual.

Na tela de Game Over, é exibida a pontuação final do jogador.

---

## Obstáculos

Os obstáculos foram pensados para combinar com o tema universitário do jogo.

Exemplos de obstáculos utilizados ou planejados:

- Mochilas;
- Catracas;
- Carteiras;
- Livros;
- Notebooks.

---

## Coletáveis

O principal coletável do jogo é o **café**.

No contexto do jogo, o café representa a energia do aluno para continuar correndo até a aula. Ao coletar cafés, o jogador aumenta sua pontuação.

---

## Telas do jogo

O projeto possui as seguintes telas/cenas:

- Tela inicial;
- Fase 1 — Tutorial;
- Fase 2 — Campus;
- Fase 3 — Chegada na aula;
- Tela de vitória;
- Tela de Game Over.

---

## Tecnologias utilizadas

- Unity;
- C#;
- TextMeshPro;
- Rigidbody2D;
- BoxCollider2D;
- Colliders e Triggers;
- Prefabs;
- Canvas;
- Sistema de cenas do Unity.

---

## Conceitos aplicados

Durante o desenvolvimento do jogo, foram aplicados conceitos trabalhados na disciplina, como:

- Criação e organização de GameObjects;
- Uso de componentes físicos em 2D;
- Controle de colisões;
- Uso de scripts em C#;
- Gerenciamento de fases;
- Controle de pontuação;
- Interface com Canvas e TextMeshPro;
- Uso de prefabs para obstáculos e coletáveis;
- Transição entre cenas;
- Criação de HUD;
- Implementação de telas de vitória e derrota.

---

## Como executar o projeto

1. Baixe ou clone este repositório:

```bash
git clone https://github.com/sophiasissi/t1-projeto-unity.git
