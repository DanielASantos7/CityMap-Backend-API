# Aplicativo de Mapa



##### 1\. Justificativa do projeto

Este projeto é a Web API para uma prova sobre um aplicativo de mapa.

Este projeto nasceu de uma prova da WorldSkills, devido á restirção de tempo da prova (máximo de 2h30 para desenvolver o backend e mobile), a estrutura do código foi focada na velocidade de desenvolvimento e no cumprimento exato dos requsitos de respostas exigidos.







##### Como executar a aplicação

###### Pré requisitos: .NET 8 SDK instalado e Microsoft SQLServer (instância local ou localDB) em execução.



###### 1\. Clone o repsitório:

git clone \[https://github.com/DanielASantos7/CityMap-Backend-API.git](https://github.com/DanielASantos7/CityMap-Backend-API.git)

cd CityMap-Backend-API



###### 2\. Prepare o Banco de Dados SQL Server:

Execute o script SQL no seu gerenciador de banco de dados para criar as tabelas de dados iniciais.

Abra o arquivo API/appsettings.json e ajuste a ConnectionString conforme a sua instância local do SQL Server



###### 3\. Restaure as dependências e inicie a API:

\# Navegue até a pasta do projeto API

cd API



\# Restaure os pacotes NuGet

dotnet restore



\# Execute a aplicação

dotnet run



###### 4\. Acesse a documentação interativa (Swagger)

Abra o navegador no endereço indicado no terminal (exemplo: http://localhost:5080/swagger)

