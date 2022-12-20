# Backend Challenge 20220626  
## This is a challenge by [Coodesh](https://coodesh.com/)


## Introdução

Nesse desafio foi desenvolvida uma REST API que utilizou os dados do projeto Open Food Facts, um banco de dados aberto com informação nutricional de diversos produtos alimentícios.

O projeto tem como objetivo dar suporte a equipe de nutricionistas da empresa Fitness Foods LC para que possam comparar de maneira rápida a informação nutricional dos alimentos da base do Open Food Facts.

### Localização 
 
- O projeto foi disponibilizado no Github, https://github.com/fernandoezequielcandido/Beltis.

### Investigação

- Durante a investigacao para o projeto foi descoberto o pacote HtmlAgilityPack que agilizou muito o densenvolvimento, com ele é possível filtrar o conteúdo do html assim localizando os itens necessários para carregamento das informações dos produtos.
- A importação dos dados está na classe Connect, de forma dividida, primeiro a obtenção da página principal, com isso vem a lista de produtos e as páginas subsequentes, com suas informações. 
## O projeto e sua execução

- Contém neste repositório o projeto do Visual Studio com ele se torna possível fazer um Deploy ou um Publish no Azure ou Docker por exemplo, mas fiz uma publicação em pasta, com isso tenho um executável que roda o serviço da API, a pasta Publish tem o conteúdo pronto para execução sem necessidade de publicar novamente, a não ser que seja o caso de mudar a forma. 
- Basta executar baixando a pasta Publish, execute Teste.exe e aguarde, uma janela de prompt ficara aberta, teremos os seguintes caminhos para teste da API:
https://localhost:5001/test
https://localhost:5001/test/products
https://localhost:5001/test/products/code sendo o code um número de produto.
- Pode-se testar estes caminhos em um navegador ou em algum Software como o Postman que traz uma lista alinhada e identada.
- Em caso de deploy ou outra forma de execução teriamos no lugar de localhost um IP OU endereço web. 
- Enquanto o prompt estiver aberto a API estara em execução
 
## Linguagem e ferramentas
- A API foi desenvolvida em C# com .Net Core 6 e Visual Studio 2022
- Como solicitado foi utilizado o Mongo DB, aponta para uma conta pessoal onde existe a base "teste_api" com a collection "products", caso seja oportuno mudar o destino dos dados basta criar uma nova base com este nome no Mongo e apontar de forma correta no appsettings. .

