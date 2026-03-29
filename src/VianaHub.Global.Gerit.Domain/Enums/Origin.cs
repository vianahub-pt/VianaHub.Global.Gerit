using System.ComponentModel;

namespace VianaHub.Global.Gerit.Domain.Enums;

public enum Origin
{
    [Description("Outros")]     // Indica que a origem do cliente não se encaixa nas categorias específicas listadas ou que o cliente não forneceu essa informação.
    Outros = 1,

    [Description("Instagram")]  // Refere-se a clientes que descobriram ou entraram em contato com a empresa por meio da plataforma de mídia social Instagram, seja por meio de anúncios, postagens, stories ou interações na plataforma.
    Instagram = 2,

    [Description("Facebook")]   // Refere-se a clientes que descobriram ou entraram em contato com a empresa por meio da plataforma de mídia social Facebook, seja por meio de anúncios, postagens, grupos ou interações na plataforma.
    Facebook = 3,

    [Description("LinkedIn")]   // Refere-se a clientes que descobriram ou entraram em contato com a empresa por meio da plataforma de rede profissional LinkedIn, seja por meio de anúncios, postagens, conexões ou interações na plataforma.
    LinkedIn = 4,

    [Description("YouTube")]    // Refere-se a clientes que descobriram ou entraram em contato com a empresa por meio da plataforma de compartilhamento de vídeos YouTube, seja por meio de anúncios, vídeos, canais ou interações na plataforma.
    YouTube = 5,

    [Description("WhatsApp")]   // Refere-se a clientes que descobriram ou entraram em contato com a empresa por meio da plataforma de mensagens instantâneas WhatsApp, seja por meio de anúncios, grupos, contatos ou interações na plataforma.
    WhatsApp = 6,

    [Description("TikTok")]     // Refere-se a clientes que descobriram ou entraram em contato com a empresa por meio da plataforma de mídia social TikTok, seja por meio de anúncios, vídeos, desafios ou interações na plataforma.
    TikTok = 7,

    [Description("Google")]     // Refere-se a clientes que descobriram ou entraram em contato com a empresa por meio do mecanismo de busca Google, seja por meio de anúncios, resultados de pesquisa orgânica ou outras formas de interação na plataforma.
    Google = 8,

    [Description("Amigos")]     // Refere-se a clientes que descobriram ou entraram em contato com a empresa por meio de recomendações ou indicações de amigos, familiares ou conhecidos, seja por meio de conversas, redes sociais ou outras formas de comunicação pessoal.
    Amigos = 9,

    [Description("TV")]         // Refere-se a clientes que descobriram ou entraram em contato com a empresa por meio de anúncios, programas ou outras formas de conteúdo veiculado na televisão.
    Tv = 10,

    [Description("Rádio")]      // Refere-se a clientes que descobriram ou entraram em contato com a empresa por meio de anúncios, programas ou outras formas de conteúdo veiculado no rádio.
    Radio = 11,

    [Description("Jornal")]     // Refere-se a clientes que descobriram ou entraram em contato com a empresa por meio de anúncios, artigos ou outras formas de conteúdo veiculado em jornais impressos ou online.
    Jornal = 12,

    [Description("Revista")]    // Refere-se a clientes que descobriram ou entraram em contato com a empresa por meio de anúncios, artigos ou outras formas de conteúdo veiculado em revistas impressas ou online.
    Revista = 13,
}
