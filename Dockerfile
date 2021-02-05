FROM mcr.microsoft.com/dotnet/sdk:5.0 AS sFlowBuild
COPY . /sFlowBuild
WORKDIR /sFlowBuild
RUN dotnet restore
RUN dotnet publish -c release -o /sFlowToElastic --no-restore
FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /sFlowToElastic
COPY --from=sFlowBuild /sFlowToElastic .
ENV ELASTIC_URI="http://elasticsearch:9200"
ENV ELASTIC_INDEX_PREFIX="ba-sflow-"
ENV ELASTIC_USER="elastic.user"
ENV ELASTIC_PASS="elastic.pass"
ENTRYPOINT ./sFlowToElasticCollector $ELASTIC_URI $ELASTIC_INDEX_PREFIX $ELASTIC_USER $ELASTIC_PASS