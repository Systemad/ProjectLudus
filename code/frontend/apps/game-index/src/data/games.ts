export type GameLink = {
    href: string;
    label: string;
};

export type SystemRequirements = {
    minimum: string[];
    recommended: string[];
};

export type Game = {
    id: string;
    title: string;
    subtitle: string;
    category: string;
    genres: string[];
    themes: string[];
    platforms: string[];
    modes: string[];
    rating: number;
    reviewLabel: string;
    image: string;
    coverImage: string;
    heroImage: string;
    summary: string;
    developer: string;
    publisher: string;
    releaseDate: string;
    requirements: SystemRequirements;
    links: GameLink[];
    screenshots: string[];
    similarGameIds: string[];
};

export const spotlightSlides = [
    {
        accent: "Ends in: 23:54:12",
        cta: "Discover",
        image: "https://lh3.googleusercontent.com/aida-public/AB6AXuAAm_37WeIymm3rYjOJtuxJIeLnxHJg_tfFLhS0TK6oZ1r4C1KyKGOPArRkrjDeWfDN_vZgQTdZoHUGj5vehws8vCi35PSQVmX-z31ULJD3icCoO8TBwi-hogPk0BLvF3-Q2n0rV3MYBxy4VfCZCtCNXKX8q5WeVxZlVfG6sxTiGJ-Cs5WvHZLmnSpTsWgUXBlmQ4IjSUYOIdu8t7O5N0iKpfcXpaK8loDc4BiW0tPzmkH7bgmrLBSoQzs2nl8jMVr7tLC4UKc0B6U",
        title: ["Starfield:", "Premium Edition"],
    },
    {
        accent: "Fresh drop",
        cta: "See campaign",
        image: "https://images.unsplash.com/photo-1560419015-7c427e8ae5ba?auto=format&fit=crop&w=1920&q=80",
        title: ["Phantom", "Liberty"],
    },
] as const;

export const anticipatedGames = [
    {
        id: "gta-vi",
        image: "https://lh3.googleusercontent.com/aida-public/AB6AXuB0XFf8nK_P-y1_qQp_Z8_m_p_r_k_v_X_p_w_x_y_z_0_1_2_3_4_5_6_7_8_9_0_1_2_3_4_5_6_7_8_9_0_1",
        meta: ["Open World Action", "PS5, Xbox Series X|S", "Rockstar Games"],
        releaseWindow: "Q3 2025",
        title: "Grand Theft Auto VI",
    },
    {
        id: "shadow-of-the-erdtree",
        image: "https://lh3.googleusercontent.com/aida-public/AB6AXuA0XFf8nK_P-y1_qQp_Z8_m_p_r_k_v_X_p_w_x_y_z_0_1_2_3_4_5_6_7_8_9_0_1_2_3_4_5_6_7_8_9_0_1_2_3_4_5_6_7_8_9_0_1",
        meta: ["Expansion / ARPG", "Single-player, Co-op", "FromSoftware"],
        releaseWindow: "June 21, 2024",
        title: "Shadow of the Erdtree",
    },
] as const;

export const games: Game[] = [
    {
        id: "hollow-knight",
        title: "Hollow Knight",
        subtitle:
            "Forge your own path through a ruined kingdom of insects, secrets, and hand-drawn danger.",
        category: "Action • Indie",
        genres: ["Action", "Adventure", "Indie"],
        themes: ["Metroidvania", "Atmospheric", "Difficult"],
        platforms: ["PS5", "Xbox", "Switch", "PC"],
        modes: ["Single-player"],
        rating: 4.9,
        reviewLabel: "Optimization Gold",
        image: "https://lh3.googleusercontent.com/aida-public/AB6AXuB_tJHo4_wD3DjObOHvvbq8mei57f5mQ0JzwzJ5gbq-qAo4ZTrfqgbQD84GNdYCFfTV8GJ33pn89XAmAsrfYRJvx5cXIQTCkoc31UwPvf8-v8vVromo-qcVmlAyDSBHKeqCDrO0q2RKjuGCg7IT5Gpw0khpUCF2MvP9kdYlxRgL0DgG_1ZrwQT3mAnxKO2B5p_d5dHWfuhQtxbm0DykWkp-3FEo0bzvDEWvXdt9LQUEAbDrBGqhFaKDOKREeZSnfhwnOvVYOKL2jjQ",
        coverImage:
            "https://lh3.googleusercontent.com/aida-public/AB6AXuDpyOGb26BfwH8IaJ6iwBy4BANWLURfU-jZa8qclHQCh4Hvj4A2FpMAlaYcYLJEkNySBtByZqIuOB06HqJOgAPlZCQQ38241ldSInf5sN9zKobPCLt7QmzlNVoRgX2ip-mUpxSUH3bNRkQs-BTZk7AJYI7PeDp2MpYLI8gBlablAfrJ51QBRBOSneVGwTtmX5kOeNjXHJY2bQVVYrW1-n4C57a7KBgEyaY_VmQpeaCHZucrHyEb_1T_2G7YFifGr6lEHk4bchUCW6Q",
        heroImage:
            "https://lh3.googleusercontent.com/aida-public/AB6AXuDpyOGb26BfwH8IaJ6iwBy4BANWLURfU-jZa8qclHQCh4Hvj4A2FpMAlaYcYLJEkNySBtByZqIuOB06HqJOgAPlZCQQ38241ldSInf5sN9zKobPCLt7QmzlNVoRgX2ip-mUpxSUH3bNRkQs-BTZk7AJYI7PeDp2MpYLI8gBlablAfrJ51QBRBOSneVGwTtmX5kOeNjXHJY2bQVVYrW1-n4C57a7KBgEyaY_VmQpeaCHZucrHyEb_1T_2G7YFifGr6lEHk4bchUCW6Q",
        summary:
            "Forge your own path in Hollow Knight, an epic action adventure through a vast ruined kingdom of insects and heroes. Explore twisting caverns, battle tainted creatures, and befriend bizarre bugs in a moody hand-drawn world.",
        developer: "Team Cherry",
        publisher: "Team Cherry",
        releaseDate: "Feb 24, 2017",
        requirements: {
            minimum: ["OS: Windows 7", "Processor: Intel Core 2 Duo E5200", "Memory: 4 GB RAM"],
            recommended: ["OS: Windows 10", "Processor: Intel Core i5", "Memory: 8 GB RAM"],
        },
        links: [
            { href: "#", label: "Steam Store" },
            { href: "#", label: "Official Website" },
            { href: "#", label: "Epic Games Store" },
        ],
        screenshots: [
            "https://lh3.googleusercontent.com/aida-public/AB6AXuAG3oApiGxL9aFzBDwpOiUNe4S4FyRglvjJY2iqLHM-_lb11wyglNWFKcYZEZ1knyUyo3yPECTNiwtIzJgLWiVrhvQ6--hdjOyb0auLSXWC9Ozk0AdohzxNZcp5O7iMT0xbP9rE8nF6J2Bb7_BqFg9jd_gPwLF8mlGlAIsbnaGevHmPSLRPpYVvR2joBlZpO8xpA2xOalUQLolhiEZ-BTMt72fik3EHgfL1woIgat_jthNEQrhZSHmzVYVA23W8UhwxGwpc9bkjWHk",
            "https://lh3.googleusercontent.com/aida-public/AB6AXuB23CWINe2onMpTVnIrQBSR0XphidAuQkpLSNXmNXynjS4l5Yk07Urn9sPYk54UHsSpxGqU_zAkGfEW1mIqkw0mxZlO1Y8h_GqRjqUIPN4begySXDzXFYudFHaqEEd3o9CSPeyOeCb2zYc7VChshBQIydqZtIvm5gJfISbZKSIuc-JnNRcn1yGEhuQl8HI7jvaYR2BVkr7IdeO8ABNUmqwGm5R6kZLXV5h5Fibljbdynh-cGXEh26ph2CGpbi-abqByh5seupYRBFE",
            "https://lh3.googleusercontent.com/aida-public/AB6AXuCHr9mQy8h3W3J1bW_N_9_2y6N8v_3X3z4e5r6t7y8u9i0o1p2a3s4d5f6g",
        ],
        similarGameIds: [
            "ori-and-the-will-of-the-wisps",
            "blasphemous-2",
            "prince-of-persia-the-lost-crown",
        ],
    },
    {
        id: "stardew-valley",
        title: "Stardew Valley",
        subtitle: "Build a quiet life, restore a farm, and turn each day into a new ritual.",
        category: "Simulation • RPG",
        genres: ["Simulation", "RPG"],
        themes: ["Cozy", "Crafting", "Community"],
        platforms: ["PC", "Switch", "PlayStation", "Xbox"],
        modes: ["Single-player", "Co-op"],
        rating: 4.9,
        reviewLabel: "Comfort Classic",
        image: "https://lh3.googleusercontent.com/aida-public/AB6AXuBohAie-JK2ssJ1Rl0-QwAVMct83sY-eWDx86biSlP4cJvHxjEage3QCGLnLgbpTTmGfksqmWF0dt-K0wr-gkeRK1fnUT_VArmAdZSBa22-WMQo_3OiAlzsSnJLyB2YZPq8YI9BiVknjihuSVrAFkuFyqgAAD_0hQxi4pd5iEvSWUm7Bi1dF7auDbyh_7-uuiIkqgfeAahFsmy1eLVqhBdVxH5fqqzu93U9jvCIOexSh8yXSGF04XKQClowgt3uh-ovnCDiCXqsbZg",
        coverImage:
            "https://lh3.googleusercontent.com/aida-public/AB6AXuBohAie-JK2ssJ1Rl0-QwAVMct83sY-eWDx86biSlP4cJvHxjEage3QCGLnLgbpTTmGfksqmWF0dt-K0wr-gkeRK1fnUT_VArmAdZSBa22-WMQo_3OiAlzsSnJLyB2YZPq8YI9BiVknjihuSVrAFkuFyqgAAD_0hQxi4pd5iEvSWUm7Bi1dF7auDbyh_7-uuiIkqgfeAahFsmy1eLVqhBdVxH5fqqzu93U9jvCIOexSh8yXSGF04XKQClowgt3uh-ovnCDiCXqsbZg",
        heroImage:
            "https://lh3.googleusercontent.com/aida-public/AB6AXuBohAie-JK2ssJ1Rl0-QwAVMct83sY-eWDx86biSlP4cJvHxjEage3QCGLnLgbpTTmGfksqmWF0dt-K0wr-gkeRK1fnUT_VArmAdZSBa22-WMQo_3OiAlzsSnJLyB2YZPq8YI9BiVknjihuSVrAFkuFyqgAAD_0hQxi4pd5iEvSWUm7Bi1dF7auDbyh_7-uuiIkqgfeAahFsmy1eLVqhBdVxH5fqqzu93U9jvCIOexSh8yXSGF04XKQClowgt3uh-ovnCDiCXqsbZg",
        summary:
            "Stardew Valley balances cozy loops, open-ended progression, and a surprising amount of personality. It remains one of the cleanest examples of a game that feels generous at every scale.",
        developer: "ConcernedApe",
        publisher: "ConcernedApe",
        releaseDate: "Feb 26, 2016",
        requirements: {
            minimum: ["OS: Windows Vista", "Processor: 2 Ghz", "Memory: 2 GB RAM"],
            recommended: ["OS: Windows 10", "Processor: 2.5 Ghz", "Memory: 4 GB RAM"],
        },
        links: [
            { href: "#", label: "Official Site" },
            { href: "#", label: "Steam Store" },
        ],
        screenshots: [
            "https://lh3.googleusercontent.com/aida-public/AB6AXuBohAie-JK2ssJ1Rl0-QwAVMct83sY-eWDx86biSlP4cJvHxjEage3QCGLnLgbpTTmGfksqmWF0dt-K0wr-gkeRK1fnUT_VArmAdZSBa22-WMQo_3OiAlzsSnJLyB2YZPq8YI9BiVknjihuSVrAFkuFyqgAAD_0hQxi4pd5iEvSWUm7Bi1dF7auDbyh_7-uuiIkqgfeAahFsmy1eLVqhBdVxH5fqqzu93U9jvCIOexSh8yXSGF04XKQClowgt3uh-ovnCDiCXqsbZg",
        ],
        similarGameIds: ["sea-of-stars", "terraria"],
    },
    {
        id: "terraria",
        title: "Terraria",
        subtitle: "A 2D sandbox where mining, boss fights, and absurd ambition coexist.",
        category: "Sandbox • Survival",
        genres: ["Sandbox", "Survival"],
        themes: ["Exploration", "Crafting", "Boss Rush"],
        platforms: ["PC", "Switch", "PlayStation", "Xbox"],
        modes: ["Single-player", "Online Co-op"],
        rating: 4.7,
        reviewLabel: "Infinite Buildcraft",
        image: "https://lh3.googleusercontent.com/aida-public/AB6AXuCf4IV8l3UEpILbulTkAW6sXj8Ezpn5h3z3gozILgAjiPxGI9kg1L8fUQLAbU5yXPWNd8TbdgY8ikb4M_KoXsAmoyu9czq9qVCfq_za1JV4l8ZOVkaf3ZLorkY4Eou4Z9l4XLPb4k8hfRGry_IwDAs2SjiZQDEy9a_fnudvf7HgCxrA-aEagXm4YHh1tnf6nbhhebpJQRt7WfaxyGOqK5PEMfpwMw7cmF0KfedCPjj218Qy80RnWCPYnP3cDcHbpd8Qfhdwfkz1Wnc",
        coverImage:
            "https://lh3.googleusercontent.com/aida-public/AB6AXuCf4IV8l3UEpILbulTkAW6sXj8Ezpn5h3z3gozILgAjiPxGI9kg1L8fUQLAbU5yXPWNd8TbdgY8ikb4M_KoXsAmoyu9czq9qVCfq_za1JV4l8ZOVkaf3ZLorkY4Eou4Z9l4XLPb4k8hfRGry_IwDAs2SjiZQDEy9a_fnudvf7HgCxrA-aEagXm4YHh1tnf6nbhhebpJQRt7WfaxyGOqK5PEMfpwMw7cmF0KfedCPjj218Qy80RnWCPYnP3cDcHbpd8Qfhdwfkz1Wnc",
        heroImage:
            "https://lh3.googleusercontent.com/aida-public/AB6AXuCf4IV8l3UEpILbulTkAW6sXj8Ezpn5h3z3gozILgAjiPxGI9kg1L8fUQLAbU5yXPWNd8TbdgY8ikb4M_KoXsAmoyu9czq9qVCfq_za1JV4l8ZOVkaf3ZLorkY4Eou4Z9l4XLPb4k8hfRGry_IwDAs2SjiZQDEy9a_fnudvf7HgCxrA-aEagXm4YHh1tnf6nbhhebpJQRt7WfaxyGOqK5PEMfpwMw7cmF0KfedCPjj218Qy80RnWCPYnP3cDcHbpd8Qfhdwfkz1Wnc",
        summary:
            "Terraria is still one of the best examples of density in a sandbox game. Every biome, build, and boss path creates another reason to push one layer deeper.",
        developer: "Re-Logic",
        publisher: "505 Games",
        releaseDate: "May 16, 2011",
        requirements: {
            minimum: ["OS: Windows XP", "Processor: 1.6 Ghz", "Memory: 1 GB RAM"],
            recommended: ["OS: Windows 10", "Processor: Dual Core", "Memory: 4 GB RAM"],
        },
        links: [
            { href: "#", label: "Official Site" },
            { href: "#", label: "Steam Store" },
        ],
        screenshots: [
            "https://lh3.googleusercontent.com/aida-public/AB6AXuCf4IV8l3UEpILbulTkAW6sXj8Ezpn5h3z3gozILgAjiPxGI9kg1L8fUQLAbU5yXPWNd8TbdgY8ikb4M_KoXsAmoyu9czq9qVCfq_za1JV4l8ZOVkaf3ZLorkY4Eou4Z9l4XLPb4k8hfRGry_IwDAs2SjiZQDEy9a_fnudvf7HgCxrA-aEagXm4YHh1tnf6nbhhebpJQRt7WfaxyGOqK5PEMfpwMw7cmF0KfedCPjj218Qy80RnWCPYnP3cDcHbpd8Qfhdwfkz1Wnc",
        ],
        similarGameIds: ["stardew-valley", "sea-of-stars"],
    },
    {
        id: "the-witcher-3",
        title: "The Witcher 3",
        subtitle: "A sprawling fantasy benchmark that still defines premium open-world questing.",
        category: "RPG • Open World",
        genres: ["RPG", "Open World"],
        themes: ["Narrative", "Fantasy", "Choice"],
        platforms: ["PC", "PlayStation", "Xbox", "Switch"],
        modes: ["Single-player"],
        rating: 4.8,
        reviewLabel: "Narrative Apex",
        image: "https://lh3.googleusercontent.com/aida-public/AB6AXuBqCJMAqehpBag4Pte5UIO9qB01VkS2LZXovaQm0PvJpEGE_Kc6lpxXJjoRy_Qk6taUoV0QFa0bdOV_G-cIDmNUCfW6II9t0-4zFrJ97weaMF1TaSqZv8uHsWuKBCC0mk3_p4lS4t2E9HA1IKo-3tkgfVv7p83ZsczHF1HGlKkfRxTlFSaI8AU2Q-WXpGeWDE6c7P5eHc2FVEQ--b8pR5rzFC6KDazcWSg_bB2kSqMMPC_c7smNwjWjrdiKf0XVV2u1aEN60hEYMBc",
        coverImage:
            "https://lh3.googleusercontent.com/aida-public/AB6AXuBqCJMAqehpBag4Pte5UIO9qB01VkS2LZXovaQm0PvJpEGE_Kc6lpxXJjoRy_Qk6taUoV0QFa0bdOV_G-cIDmNUCfW6II9t0-4zFrJ97weaMF1TaSqZv8uHsWuKBCC0mk3_p4lS4t2E9HA1IKo-3tkgfVv7p83ZsczHF1HGlKkfRxTlFSaI8AU2Q-WXpGeWDE6c7P5eHc2FVEQ--b8pR5rzFC6KDazcWSg_bB2kSqMMPC_c7smNwjWjrdiKf0XVV2u1aEN60hEYMBc",
        heroImage:
            "https://lh3.googleusercontent.com/aida-public/AB6AXuBqCJMAqehpBag4Pte5UIO9qB01VkS2LZXovaQm0PvJpEGE_Kc6lpxXJjoRy_Qk6taUoV0QFa0bdOV_G-cIDmNUCfW6II9t0-4zFrJ97weaMF1TaSqZv8uHsWuKBCC0mk3_p4lS4t2E9HA1IKo-3tkgfVv7p83ZsczHF1HGlKkfRxTlFSaI8AU2Q-WXpGeWDE6c7P5eHc2FVEQ--b8pR5rzFC6KDazcWSg_bB2kSqMMPC_c7smNwjWjrdiKf0XVV2u1aEN60hEYMBc",
        summary:
            "The Witcher 3 is still a reference point for how to make side content feel authored instead of disposable. It remains a useful contrast to more systems-heavy RPGs.",
        developer: "CD Projekt Red",
        publisher: "CD Projekt",
        releaseDate: "May 18, 2015",
        requirements: {
            minimum: [
                "OS: 64-bit Windows 7",
                "Processor: Intel CPU Core i5-2500K",
                "Memory: 6 GB RAM",
            ],
            recommended: [
                "OS: 64-bit Windows 10",
                "Processor: Intel CPU Core i7 3770",
                "Memory: 8 GB RAM",
            ],
        },
        links: [
            { href: "#", label: "Official Site" },
            { href: "#", label: "GOG Store" },
        ],
        screenshots: [
            "https://lh3.googleusercontent.com/aida-public/AB6AXuBqCJMAqehpBag4Pte5UIO9qB01VkS2LZXovaQm0PvJpEGE_Kc6lpxXJjoRy_Qk6taUoV0QFa0bdOV_G-cIDmNUCfW6II9t0-4zFrJ97weaMF1TaSqZv8uHsWuKBCC0mk3_p4lS4t2E9HA1IKo-3tkgfVv7p83ZsczHF1HGlKkfRxTlFSaI8AU2Q-WXpGeWDE6c7P5eHc2FVEQ--b8pR5rzFC6KDazcWSg_bB2kSqMMPC_c7smNwjWjrdiKf0XVV2u1aEN60hEYMBc",
        ],
        similarGameIds: ["hollow-knight", "sea-of-stars"],
    },
    {
        id: "sea-of-stars",
        title: "Sea of Stars",
        subtitle:
            "A polished retro RPG that feels editorially curated instead of nostalgic by default.",
        category: "JRPG • Turn-Based",
        genres: ["JRPG", "Turn-Based"],
        themes: ["Retro", "Bright World", "Party Adventure"],
        platforms: ["PC", "Switch", "PlayStation", "Xbox"],
        modes: ["Single-player"],
        rating: 4.7,
        reviewLabel: "Modern Retro Precision",
        image: "https://lh3.googleusercontent.com/aida-public/AB6AXuASlFSBq4DGxHqJETp-BiAGYxvIIXGV6xX8_EB38Un0Ym8aUJUCRJ5lPCJyLsxEqDprXT7Q6xXj1B67feh4IK7HXB3zk03NxNN5KniKNuVW3qaRXKtDQWep0tl7bIrtKgf3LrzveAkXe6BGL5lZDoEAAaQ0f3wiGG9ELjCr5WORgVAHMTZ6ZnTz5jGQo1xoyRjIkcAgDJy32j60_w3YWshDmnkAaX_A4twVFgc9aJ0heMp_IxZfgKkFxCJXXGi2JSsqlxhJNiONcCI",
        coverImage:
            "https://lh3.googleusercontent.com/aida-public/AB6AXuASlFSBq4DGxHqJETp-BiAGYxvIIXGV6xX8_EB38Un0Ym8aUJUCRJ5lPCJyLsxEqDprXT7Q6xXj1B67feh4IK7HXB3zk03NxNN5KniKNuVW3qaRXKtDQWep0tl7bIrtKgf3LrzveAkXe6BGL5lZDoEAAaQ0f3wiGG9ELjCr5WORgVAHMTZ6ZnTz5jGQo1xoyRjIkcAgDJy32j60_w3YWshDmnkAaX_A4twVFgc9aJ0heMp_IxZfgKkFxCJXXGi2JSsqlxhJNiONcCI",
        heroImage:
            "https://lh3.googleusercontent.com/aida-public/AB6AXuASlFSBq4DGxHqJETp-BiAGYxvIIXGV6xX8_EB38Un0Ym8aUJUCRJ5lPCJyLsxEqDprXT7Q6xXj1B67feh4IK7HXB3zk03NxNN5KniKNuVW3qaRXKtDQWep0tl7bIrtKgf3LrzveAkXe6BGL5lZDoEAAaQ0f3wiGG9ELjCr5WORgVAHMTZ6ZnTz5jGQo1xoyRjIkcAgDJy32j60_w3YWshDmnkAaX_A4twVFgc9aJ0heMp_IxZfgKkFxCJXXGi2JSsqlxhJNiONcCI",
        summary:
            "Sea of Stars lands in the exact place where presentation, progression, and accessibility intersect cleanly. It makes for strong card-based discovery because it reads instantly.",
        developer: "Sabotage Studio",
        publisher: "Sabotage Studio",
        releaseDate: "Aug 29, 2023",
        requirements: {
            minimum: ["OS: Windows 7", "Processor: Intel Core i3", "Memory: 4 GB RAM"],
            recommended: ["OS: Windows 10", "Processor: Intel Core i5", "Memory: 8 GB RAM"],
        },
        links: [
            { href: "#", label: "Official Site" },
            { href: "#", label: "Nintendo eShop" },
        ],
        screenshots: [
            "https://lh3.googleusercontent.com/aida-public/AB6AXuASlFSBq4DGxHqJETp-BiAGYxvIIXGV6xX8_EB38Un0Ym8aUJUCRJ5lPCJyLsxEqDprXT7Q6xXj1B67feh4IK7HXB3zk03NxNN5KniKNuVW3qaRXKtDQWep0tl7bIrtKgf3LrzveAkXe6BGL5lZDoEAAaQ0f3wiGG9ELjCr5WORgVAHMTZ6ZnTz5jGQo1xoyRjIkcAgDJy32j60_w3YWshDmnkAaX_A4twVFgc9aJ0heMp_IxZfgKkFxCJXXGi2JSsqlxhJNiONcCI",
        ],
        similarGameIds: ["stardew-valley", "the-witcher-3"],
    },
    {
        id: "ori-and-the-will-of-the-wisps",
        title: "Ori and the Will of the Wisps",
        subtitle: "A luminous action platformer with precision movement and lavish motion.",
        category: "Action • Platformer",
        genres: ["Action", "Platformer"],
        themes: ["Atmospheric", "Precision", "Emotional"],
        platforms: ["PC", "Xbox", "Switch"],
        modes: ["Single-player"],
        rating: 4.8,
        reviewLabel: "Visual Poetry",
        image: "https://lh3.googleusercontent.com/aida-public/AB6AXuCHr9mQy8h3W3J1bW_N_9_2y6N8v_3X3z4e5r6t7y8u9i0o1p2a3s4d5f6g",
        coverImage:
            "https://lh3.googleusercontent.com/aida-public/AB6AXuCHr9mQy8h3W3J1bW_N_9_2y6N8v_3X3z4e5r6t7y8u9i0o1p2a3s4d5f6g",
        heroImage:
            "https://lh3.googleusercontent.com/aida-public/AB6AXuCHr9mQy8h3W3J1bW_N_9_2y6N8v_3X3z4e5r6t7y8u9i0o1p2a3s4d5f6g",
        summary:
            "Ori pairs painterly presentation with exacting traversal, making it a natural related pick for Hollow Knight readers.",
        developer: "Moon Studios",
        publisher: "Xbox Game Studios",
        releaseDate: "Mar 11, 2020",
        requirements: {
            minimum: ["OS: Windows 10", "Processor: AMD Athlon X4", "Memory: 8 GB RAM"],
            recommended: ["OS: Windows 10", "Processor: Intel i5", "Memory: 8 GB RAM"],
        },
        links: [{ href: "#", label: "Official Site" }],
        screenshots: [
            "https://lh3.googleusercontent.com/aida-public/AB6AXuCHr9mQy8h3W3J1bW_N_9_2y6N8v_3X3z4e5r6t7y8u9i0o1p2a3s4d5f6g",
        ],
        similarGameIds: ["hollow-knight"],
    },
    {
        id: "blasphemous-2",
        title: "Blasphemous 2",
        subtitle: "Severe art direction and bruising combat in a dense 2D pilgrimage.",
        category: "Action • Soulslike",
        genres: ["Action", "Soulslike"],
        themes: ["Gothic", "Punishing", "Pilgrimage"],
        platforms: ["PC", "Switch", "PlayStation", "Xbox"],
        modes: ["Single-player"],
        rating: 4.5,
        reviewLabel: "Dark Devotion",
        image: "https://images.unsplash.com/photo-1542751371-adc38448a05e?auto=format&fit=crop&w=900&q=80",
        coverImage:
            "https://images.unsplash.com/photo-1542751371-adc38448a05e?auto=format&fit=crop&w=1200&q=80",
        heroImage:
            "https://images.unsplash.com/photo-1542751371-adc38448a05e?auto=format&fit=crop&w=1600&q=80",
        summary:
            "Blasphemous 2 is a sharper, faster recommendation when the audience wants atmosphere and punishment over exploration breadth.",
        developer: "The Game Kitchen",
        publisher: "Team17",
        releaseDate: "Aug 24, 2023",
        requirements: {
            minimum: ["OS: Windows 10", "Processor: Intel Core i3-9100", "Memory: 8 GB RAM"],
            recommended: ["OS: Windows 11", "Processor: Intel Core i5-9600", "Memory: 16 GB RAM"],
        },
        links: [{ href: "#", label: "Official Site" }],
        screenshots: [
            "https://images.unsplash.com/photo-1542751371-adc38448a05e?auto=format&fit=crop&w=1200&q=80",
        ],
        similarGameIds: ["hollow-knight"],
    },
    {
        id: "prince-of-persia-the-lost-crown",
        title: "Prince of Persia: The Lost Crown",
        subtitle: "An athletic, modern metroidvania with clear combat feedback and velocity.",
        category: "Action • Metroidvania",
        genres: ["Action", "Metroidvania"],
        themes: ["Traversal", "Combat", "Mythic"],
        platforms: ["PC", "Switch", "PlayStation", "Xbox"],
        modes: ["Single-player"],
        rating: 4.6,
        reviewLabel: "Traversal Machine",
        image: "https://images.unsplash.com/photo-1511512578047-dfb367046420?auto=format&fit=crop&w=900&q=80",
        coverImage:
            "https://images.unsplash.com/photo-1511512578047-dfb367046420?auto=format&fit=crop&w=1200&q=80",
        heroImage:
            "https://images.unsplash.com/photo-1511512578047-dfb367046420?auto=format&fit=crop&w=1600&q=80",
        summary:
            "The Lost Crown is a strong recommendation when the audience cares more about movement fluency than melancholy worldbuilding.",
        developer: "Ubisoft Montpellier",
        publisher: "Ubisoft",
        releaseDate: "Jan 18, 2024",
        requirements: {
            minimum: ["OS: Windows 10", "Processor: Intel Core i5-4460", "Memory: 8 GB RAM"],
            recommended: ["OS: Windows 11", "Processor: Intel Core i7-6700", "Memory: 16 GB RAM"],
        },
        links: [{ href: "#", label: "Official Site" }],
        screenshots: [
            "https://images.unsplash.com/photo-1511512578047-dfb367046420?auto=format&fit=crop&w=1200&q=80",
        ],
        similarGameIds: ["hollow-knight"],
    },
];

export const trendingGameIds = [
    "hollow-knight",
    "stardew-valley",
    "terraria",
    "the-witcher-3",
] as const;

export const searchCollections = {
    availability: ["Released", "Coming Soon"],
    genres: ["RPG", "Action", "Indie", "Strategy", "JRPG", "Simulation"],
    platforms: ["PlayStation", "Xbox", "Switch", "PC"],
};

export function getGameById(gameId: string) {
    return games.find((game) => game.id === gameId);
}

export function getGamesByIds(gameIds: string[]) {
    return gameIds.map((gameId) => getGameById(gameId)).filter((game) => game != null);
}

export const spotlightIndies = [
    {
        id: "celeste",
        title: "Celeste",
        category: "Platformer",
        subtitle:
            "A precision platformer built around movement mastery and quiet emotional courage.",
        rating: 4.9,
        image: "https://images.unsplash.com/photo-1519681393784-d120267933ba?auto=format&fit=crop&w=400&q=80",
    },
    {
        id: "outer-wilds",
        title: "Outer Wilds",
        category: "Exploration",
        subtitle:
            "A solar system mystery that rewards curiosity and careful observation over combat.",
        rating: 4.8,
        image: "https://images.unsplash.com/photo-1446776877081-d282a0f896e2?auto=format&fit=crop&w=400&q=80",
    },
    {
        id: "tunic",
        title: "Tunic",
        category: "Adventure",
        subtitle: "A fox in a strange land learns its own rules, hidden inside a cryptic manual.",
        rating: 4.6,
        image: "https://images.unsplash.com/photo-1500534314209-a25ddb2bd429?auto=format&fit=crop&w=400&q=80",
    },
    {
        id: "hades",
        title: "Hades",
        category: "Roguelike",
        subtitle: "Every death advances the narrative in a roguelike built around story momentum.",
        rating: 4.9,
        image: "https://images.unsplash.com/photo-1518709268805-4e9042af9f23?auto=format&fit=crop&w=400&q=80",
    },
] as const;

export const highestRated = [
    {
        id: "breath-of-the-wild",
        subtitle: "The Legend of Zelda",
        title: "Breath of the Wild",
        score: 97,
        image: "https://images.unsplash.com/photo-1535223289429-462edb4a5e77?auto=format&fit=crop&w=400&q=80",
    },
    {
        id: "god-of-war-ragnarok",
        subtitle: "God of War",
        title: "Ragnarök",
        score: 94,
        image: "https://images.unsplash.com/photo-1509198397868-475647b2a1e5?auto=format&fit=crop&w=400&q=80",
    },
    {
        id: "elden-ring",
        subtitle: "",
        title: "Elden Ring",
        score: 96,
        image: "https://images.unsplash.com/photo-1552820728-8b83bb6b773f?auto=format&fit=crop&w=400&q=80",
    },
] as const;

export const releasingThisMonth = [
    {
        id: "hollow-knight",
        title: "Hollow Knight",
        releaseDate: "Jan 18, 2022",
        image: "https://lh3.googleusercontent.com/aida-public/AB6AXuDpyOGb26BfwH8IaJ6iwBy4BANWLURfU-jZa8qclHQCh4Hvj4A2FpMAlaYcYLJEkNySBtByZqIuOB06HqJOgAPlZCQQ38241ldSInf5sN9zKobPCLt7QmzlNVoRgX2ip-mUpxSUH3bNRkQs-BTZk7AJYI7PeDp2MpYLI8gBlablAfrJ51QBRBOSneVGwTtmX5kOeNjXHJY2bQVVYrW1-n4C57a7KBgEyaY_VmQpeaCHZucrHyEb_1T_2G7YFifGr6lEHk4bchUCW6Q",
    },
    {
        id: "terraria",
        title: "Terraria",
        releaseDate: "Jan 20, 2022",
        image: "https://lh3.googleusercontent.com/aida-public/AB6AXuCf4IV8l3UEpILbulTkAW6sXj8Ezpn5h3z3gozILgAjiPxGI9kg1L8fUQLAbU5yXPWNd8TbdgY8ikb4M_KoXsAmoyu9czq9qVCfq_za1JV4l8ZOVkaf3ZLorkY4Eou4Z9l4XLPb4k8hfRGry_IwDAs2SjiZQDEy9a_fnudvf7HgCxrA-aEagXm4YHh1tnf6nbhhebpJQRt7WfaxyGOqK5PEMfpwMw7cmF0KfedCPjj218Qy80RnWCPYnP3cDcHbpd8Qfhdwfkz1Wnc",
    },
    {
        id: "hades-ii",
        title: "Hades II",
        releaseDate: "Jan 22, 2022",
        image: "https://images.unsplash.com/photo-1518709268805-4e9042af9f23?auto=format&fit=crop&w=200&q=80",
    },
    {
        id: "stardew-valley",
        title: "Stardew Valley",
        releaseDate: "Jan 24, 2022",
        image: "https://lh3.googleusercontent.com/aida-public/AB6AXuBohAie-JK2ssJ1Rl0-QwAVMct83sY-eWDx86biSlP4cJvHxjEage3QCGLnLgbpTTmGfksqmWF0dt-K0wr-gkeRK1fnUT_VArmAdZSBa22-WMQo_3OiAlzsSnJLyB2YZPq8YI9BiVknjihuSVrAFkuFyqgAAD_0hQxi4pd5iEvSWUm7Bi1dF7auDbyh_7-uuiIkqgfeAahFsmy1eLVqhBdVxH5fqqzu93U9jvCIOexSh8yXSGF04XKQClowgt3uh-ovnCDiCXqsbZg",
    },
    {
        id: "sea-of-stars",
        title: "Sea of Stars",
        releaseDate: "Jan 26, 2022",
        image: "https://lh3.googleusercontent.com/aida-public/AB6AXuASlFSBq4DGxHqJETp-BiAGYxvIIXGV6xX8_EB38Un0Ym8aUJUCRJ5lPCJyLsxEqDprXT7Q6xXj1B67feh4IK7HXB3zk03NxNN5KniKNuVW3qaRXKtDQWep0tl7bIrtKgf3LrzveAkXe6BGL5lZDoEAAaQ0f3wiGG9ELjCr5WORgVAHMTZ6ZnTz5jGQo1xoyRjIkcAgDJy32j60_w3YWshDmnkAaX_A4twVFgc9aJ0heMp_IxZfgKkFxCJXXGi2JSsqlxhJNiONcCI",
    },
    {
        id: "the-witcher-3",
        title: "The Witcher 3",
        releaseDate: "Jan 28, 2022",
        image: "https://lh3.googleusercontent.com/aida-public/AB6AXuBqCJMAqehpBag4Pte5UIO9qB01VkS2LZXovaQm0PvJpEGE_Kc6lpxXJjoRy_Qk6taUoV0QFa0bdOV_G-cIDmNUCfW6II9t0-4zFrJ97weaMF1TaSqZv8uHsWuKBCC0mk3_p4lS4t2E9HA1IKo-3tkgfVv7p83ZsczHF1HGlKkfRxTlFSaI8AU2Q-WXpGeWDE6c7P5eHc2FVEQ--b8pR5rzFC6KDazcWSg_bB2kSqMMPC_c7smNwjWjrdiKf0XVV2u1aEN60hEYMBc",
    },
] as const;
