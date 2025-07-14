import { createFileRoute } from "@tanstack/react-router";
import { Accordion, AccordionItem, Flex } from "@yamada-ui/react";

export const Route = createFileRoute("/games/")({
    component: RouteComponent,
});

/*
const [search, setSearch] = useState('');
const [selectedGenres, setSelectedGenres] = useState([]);
const [selectedPlatforms, setSelectedPlatforms] = useState([]);

const queryKey = [
  'games',
  { search, genres: selectedGenres, platforms: selectedPlatforms }
];

const { data, isLoading } = useQuery({
  queryKey,
  queryFn: () => fetchGames({ search, genres: selectedGenres, platforms: selectedPlatforms }),
});
*/

// TODO: Get Platforms etc for filters
// when clicking a checkbox, just add the ID to use state
// then use onChange to handle API call to server
function RouteComponent() {
    //const searchQuuery =
    return (
        <>
            <Flex direction="column" w="full">
                {/* Top Bar */}
                <Flex
                    borderRadius={"xl"}
                    w="full"
                    bg={["blackAlpha.50", "whiteAlpha.100"]}
                    h="5xs"
                    align="center"
                    justify="center"
                >
                    {/* Top bar content here */}
                </Flex>

                {/* Main Content Split */}
                <Flex
                    borderRadius={"xl"}
                    gap="md"
                    mt="4"
                    w="full"
                    minH="md"
                    h="full"
                >
                    {/* Left Side: 1/4 width */}
                    <Flex
                        borderRadius={"xl"}
                        flex="1"
                        bg={["blackAlpha.50", "whiteAlpha.100"]}
                        p="4"
                    >
                        <Accordion
                            defaultIndex={[0, 1, 2]}
                            multiple={true}
                            variant="card"
                        >
                            <AccordionItem
                                rounded="xl"
                                label="Platforms"
                            ></AccordionItem>

                            <AccordionItem
                                rounded="xl"
                                label="Game Launchers"
                            ></AccordionItem>

                            <AccordionItem
                                rounded="xl"
                                label="Release dates"
                            ></AccordionItem>
                        </Accordion>
                    </Flex>
                    {/* Right Side: 3/4 width */}
                    <Flex
                        borderRadius={"xl"}
                        flex="3"
                        bg={["blackAlpha.50", "whiteAlpha.100"]}
                        p="4"
                    >
                        {/* Right content */}
                    </Flex>
                </Flex>
            </Flex>
        </>
    );
}
