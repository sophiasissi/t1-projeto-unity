# Corre, Calouro! 🎮☕

## Integrantes

- **Sophia Sissi Curcio Guedes** — 23.01044-4
- **Gustavo Coutinho Arruda** — 23.00938-0

---

## Sobre o projeto

**Corre, Calouro!** é um jogo 2D desenvolvido em **Unity** para a disciplina de **Computação Gráfica e Desenvolvimento de Jogos / CIC402 — Projeto T1**.

O projeto foi criado com o objetivo de desenvolver um jogo funcional, jogável e com uma primeira fase em formato de tutorial, conforme a proposta da disciplina.

A ideia do jogo é trazer uma experiência simples de entender, mas com mecânicas suficientes para gerar desafio, progressão e interação com o jogador.

---

## Objetivo do jogo

O jogador controla um aluno atrasado que precisa atravessar o campus da faculdade e chegar a tempo na aula.

Durante o percurso, é necessário:

- Desviar de obstáculos;
- Pular objetos no caminho;
- Coletar cafés;
- Acumular pontos;
- Avançar pelas fases até chegar ao destino final.

---

## Inspiração

O jogo foi inspirado em jogos de corrida infinita, principalmente no estilo de **Subway Surfers**, mas adaptado para uma proposta 2D com tema universitário.

---

## Mecânicas principais

O jogo possui as seguintes mecânicas:

- Corrida automática do personagem;
- Movimentação lateral entre faixas;
- Pulo para desviar de obstáculos;
- Coleta de cafés;
- Sistema de pontuação;
- Contador de cafés coletados;
- Fases com dificuldade progressiva;
- Tela inicial;
- Tela de Game Over;
- Tela de vitória;
- Primeira fase em formato de tutorial.

---

## Controles

| Tecla | Ação |
|------|------|
| `A` ou seta esquerda | Move o personagem para a esquerda |
| `D` ou seta direita | Move o personagem para a direita |
| `Espaço` ou seta para cima | Faz o personagem pular |

---

## Estrutura das fases

### Fase 1 — Tutorial

A primeira fase funciona como uma fase demonstrativa, ensinando os comandos básicos do jogo enquanto o jogador joga.

Durante essa fase, o jogador aprende a:

- Mover para a esquerda;
- Mover para a direita;
- Pular;
- Coletar cafés;
- Evitar obstáculos.

Essa fase foi criada para atender ao requisito da disciplina de que a primeira fase funcione como tutorial para o restante do jogo.

---

### Fase 2 — Campus

A segunda fase representa a corrida pelo campus da faculdade.

Nessa fase, o jogador já precisa jogar sem instruções guiadas, desviando dos obstáculos e coletando cafés para aumentar sua pontuação.

Características da fase:

- Velocidade intermediária;
- Obstáculos mais frequentes;
- Cafés posicionados pelo caminho;
- Maior necessidade de reação do jogador.

---

### Fase 3 — Chegada na aula

A terceira fase representa a parte final do percurso, próxima à sala de aula.

Essa fase possui maior dificuldade, exigindo mais atenção e reflexo do jogador.

Características da fase:

- Velocidade maior;
- Mais obstáculos;
- Menor tempo de reação;
- Maior desafio para coletar cafés.

Ao concluir essa fase, o jogador vence o jogo.

---

## Pontuação

O jogador ganha pontos ao coletar cafés durante as fases.

A interface do jogo exibe:

- Pontuação atual;
- Quantidade de cafés coletados;
- Nome da fase atual.

Na tela de **Game Over**, é exibida a pontuação final do jogador.

---

## Obstáculos

Os obstáculos foram escolhidos para combinar com o tema universitário do jogo.

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

O jogo possui as seguintes telas/cenas:

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
