﻿namespace Maps;
internal class Helpers
{
    public static List<KeyValuePair<string, string>> Substitutions = new List<KeyValuePair<string, string>>
    {
        new("ALLEE", "ALLEY"),
        new("ALLY", "ALLEY"),
        new("ALY", "ALLEY"),
        new("ANNEX", "ANEX"),
        new("ANNX", "ANEX"),
        new("ANX", "ANEX"),
        new("ARC", "ARCADE"),
        new("AV", "AVENUE"),
        new("AVE", "AVENUE"),
        new("AVEN", "AVENUE"),
        new("AVENU", "AVENUE"),
        new("AVN", "AVENUE"),
        new("AVNUE", "AVENUE"),
        new("BAYOO", "BAYOU"),
        new("BCH", "BEACH"),
        new("BND", "BEND"),
        new("BLF", "BLUFF"),
        new("BLUF", "BLUFF"),
        new("BOT", "BOTTOM"),
        new("BTM", "BOTTOM"),
        new("BOTTM", "BOTTOM"),
        new("BLVD", "BOULEVARD"),
        new("BOUL", "BOULEVARD"),
        new("BOULV", "BOULEVARD"),
        new("BR", "BRANCH"),
        new("BRNCH", "BRANCH"),
        new("BRDGE", "BRIDGE"),
        new("BRG", "BRIDGE"),
        new("BRK", "BROOK"),
        new("BYP", "BYPASS"),
        new("BYPA", "BYPASS"),
        new("BYPAS", "BYPASS"),
        new("BYPS", "BYPASS"),
        new("CP", "CAMP"),
        new("CMP", "CAMP"),
        new("CANYN", "CANYON"),
        new("CNYN", "CANYON"),
        new("CPE", "CAPE"),
        new("CAUSWA", "CAUSEWAY"),
        new("CSWY", "CAUSEWAY"),
        new("CEN", "CENTER"),
        new("CENT", "CENTER"),
        new("CENTR", "CENTER"),
        new("CENTRE", "CENTER"),
        new("CNTER", "CENTER"),
        new("CNTR", "CENTER"),
        new("CTR", "CENTER"),
        new("CIR", "CIRCLE"),
        new("CIRC", "CIRCLE"),
        new("CIRCL", "CIRCLE"),
        new("CRCL", "CIRCLE"),
        new("CRCLE", "CIRCLE"),
        new("CLF", "CLIFF"),
        new("CLFS", "CLIFFS"),
        new("CLB", "CLUB"),
        new("COR", "CORNER"),
        new("CORS", "CORNERS"),
        new("CRSE", "COURSE"),
        new("CT", "COURT"),
        new("CTS", "COURTS"),
        new("CV", "COVE"),
        new("CRK", "CREEK"),
        new("CRES", "CRESCENT"),
        new("CRSENT", "CRESCENT"),
        new("CRSNT", "CRESCENT"),
        new("CRSSNG", "CROSSING"),
        new("XING", "CROSSING"),
        new("DL", "DALE"),
        new("DM", "DAM"),
        new("DIV", "DIVIDE"),
        new("DV", "DIVIDE"),
        new("DVD", "DIVIDE"),
        new("DR", "DRIVE"),
        new("DRIV", "DRIVE"),
        new("DRV", "DRIVE"),
        new("EST", "ESTATE"),
        new("ESTS", "ESTATES"),
        new("EXP", "EXPRESSWAY"),
        new("EXPR", "EXPRESSWAY"),
        new("EXPRESS", "EXPRESSWAY"),
        new("EXPW", "EXPRESSWAY"),
        new("EXPY", "EXPRESSWAY"),
        new("EXT", "EXTENSION"),
        new("EXTN", "EXTENSION"),
        new("EXTNSN", "EXTENSION"),
        new("EXTS", "EXTENSIONS"),
        new("FLS", "FALLS"),
        new("FRRY", "FERRY"),
        new("FRY", "FERRY"),
        new("FLD", "FIELD"),
        new("FLDS", "FIELDS"),
        new("FLT", "FLAT"),
        new("FLTS", "FLATS"),
        new("FRD", "FORD"),
        new("FORESTS", "FOREST"),
        new("FRST", "FOREST"),
        new("FORG", "FORGE"),
        new("FRG", "FORGE"),
        new("FRK", "FORK"),
        new("FRKS", "FORKS"),
        new("FRT", "FORT"),
        new("FT", "FORT"),
        new("FREEWY", "FREEWAY"),
        new("FRWAY", "FREEWAY"),
        new("FRWY", "FREEWAY"),
        new("FWY", "FREEWAY"),
        new("GARDN", "GARDEN"),
        new("GRDEN", "GARDEN"),
        new("GRDN", "GARDEN"),
        new("GDNS", "GARDENS"),
        new("GRDNS", "GARDENS"),
        new("GATEWY", "GATEWAY"),
        new("GATWAY", "GATEWAY"),
        new("GTWAY", "GATEWAY"),
        new("GTWY", "GATEWAY"),
        new("GLN", "GLEN"),
        new("GRN", "GREEN"),
        new("GROV", "GROVE"),
        new("GRV", "GROVE"),
        new("HARB", "HARBOR"),
        new("HARBR", "HARBOR"),
        new("HBR", "HARBOR"),
        new("HRBOR", "HARBOR"),
        new("HVN", "HAVEN"),
        new("HT", "HEIGHTS"),
        new("HTS", "HEIGHTS"),
        new("HIGHWY", "HIGHWAY"),
        new("HIWAY", "HIGHWAY"),
        new("HIWY", "HIGHWAY"),
        new("HWAY", "HIGHWAY"),
        new("HWY", "HIGHWAY"),
        new("HL", "HILL"),
        new("HLS", "HILLS"),
        new("HLLW", "HOLLOW"),
        new("HOLLOWS", "HOLLOW"),
        new("HOLW", "HOLLOW"),
        new("HOLWS", "HOLLOW"),
        new("INLT", "INLET"),
        new("IS", "ISLAND"),
        new("ISLND", "ISLAND"),
        new("ISLNDS", "ISLANDS"),
        new("ISS", "ISLANDS"),
        new("ISLES", "ISLE"),
        new("JCT", "JUNCTION"),
        new("JCTION", "JUNCTION"),
        new("JCTN", "JUNCTION"),
        new("JUNCTN", "JUNCTION"),
        new("JUNCTON", "JUNCTION"),
        new("JCTNS", "JUNCTIONS"),
        new("JCTS", "JUNCTIONS"),
        new("KY", "KEY"),
        new("KYS", "KEYS"),
        new("KNL", "KNOLL"),
        new("KNOL", "KNOLL"),
        new("KNLS", "KNOLLS"),
        new("LK", "LAKE"),
        new("LKS", "LAKES"),
        new("LNDG", "LANDING"),
        new("LNDNG", "LANDING"),
        new("LN", "LANE"),
        new("LGT", "LIGHT"),
        new("LF", "LOAF"),
        new("LCK", "LOCK"),
        new("LCKS", "LOCKS"),
        new("LDG", "LODGE"),
        new("LDGE", "LODGE"),
        new("LODG", "LODGE"),
        new("LOOPS", "LOOP"),
        new("MNR", "MANOR"),
        new("MNRS", "MANORS"),
        new("MDW", "MEADOWS"),
        new("MDWS", "MEADOWS"),
        new("MEDOWS", "MEADOWS"),
        new("MISSN", "MISSION"),
        new("MSSN", "MISSION"),
        new("MNT", "MOUNT"),
        new("MT", "MOUNT"),
        new("MNTAIN", "MOUNTAIN"),
        new("MNTN", "MOUNTAIN"),
        new("MOUNTIN", "MOUNTAIN"),
        new("MTIN", "MOUNTAIN"),
        new("MTN", "MOUNTAIN"),
        new("MNTNS", "MOUNTAINS"),
        new("NCK", "NECK"),
        new("ORCH", "ORCHARD"),
        new("ORCHRD", "ORCHARD"),
        new("OVL", "OVAL"),
        new("PRK", "PARK"),
        new("PARKWY", "PARKWAY"),
        new("PKWAY", "PARKWAY"),
        new("PKWY", "PARKWAY"),
        new("PKY", "PARKWAY"),
        new("PKWYS", "PARKWAYS"),
        new("PATHS", "PATH"),
        new("PIKES", "PIKE"),
        new("PNES", "PINES"),
        new("PL", "PLACE"),
        new("PLN", "PLAIN"),
        new("PLNS", "PLAINS"),
        new("PLZ", "PLAZA"),
        new("PLZA", "PLAZA"),
        new("PT", "POINT"),
        new("PTS", "POINTS"),
        new("PRT", "PORT"),
        new("PRTS", "PORTS"),
        new("PR", "PRAIRIE"),
        new("PRR", "PRAIRIE"),
        new("RAD", "RADIAL"),
        new("RADIEL", "RADIAL"),
        new("RADL", "RADIAL"),
        new("RANCHES", "RANCH"),
        new("RNCH", "RANCH"),
        new("RNCHS", "RANCH"),
        new("RPD", "RAPID"),
        new("RPDS", "RAPIDS"),
        new("RST", "REST"),
        new("RDG", "RIDGE"),
        new("RDGE", "RIDGE"),
        new("RDGS", "RIDGES"),
        new("RIV", "RIVER"),
        new("RVR", "RIVER"),
        new("RIVR", "RIVER"),
        new("RD", "ROAD"),
        new("RDS", "ROADS"),
        new("SHL", "SHOAL"),
        new("SHLS", "SHOALS"),
        new("SHOAR", "SHORE"),
        new("SHR", "SHORE"),
        new("SHOARS", "SHORES"),
        new("SHRS", "SHORES"),
        new("SPG", "SPRING"),
        new("SPNG", "SPRING"),
        new("SPRNG", "SPRING"),
        new("SPGS", "SPRINGS"),
        new("SPNGS", "SPRINGS"),
        new("SPRNGS", "SPRINGS"),
        new("SQ", "SQUARE"),
        new("SQR", "SQUARE"),
        new("SQRE", "SQUARE"),
        new("SQU", "SQUARE"),
        new("SQRS", "SQUARES"),
        new("STA", "STATION"),
        new("STATN", "STATION"),
        new("STN", "STATION"),
        new("STRA", "STRAVENUE"),
        new("STRAV", "STRAVENUE"),
        new("STRAVEN", "STRAVENUE"),
        new("STRAVN", "STRAVENUE"),
        new("STRVN", "STRAVENUE"),
        new("STRVNUE", "STRAVENUE"),
        new("STREME", "STREAM"),
        new("STRM", "STREAM"),
        new("STRT", "STREET"),
        new("ST", "STREET"),
        new("STR", "STREET"),
        new("SMT", "SUMMIT"),
        new("SUMIT", "SUMMIT"),
        new("SUMITT", "SUMMIT"),
        new("TER", "TERRACE"),
        new("TERR", "TERRACE"),
        new("TRACES", "TRACE"),
        new("TRCE", "TRACE"),
        new("TRACKS", "TRACK"),
        new("TRAK", "TRACK"),
        new("TRK", "TRACK"),
        new("TRKS", "TRACK"),
        new("TRAILS", "TRAIL"),
        new("TRL", "TRAIL"),
        new("TRLS", "TRAIL"),
        new("TRLR", "TRAILER"),
        new("TRLRS", "TRAILER"),
        new("TUNEL", "TUNNEL"),
        new("TUNL", "TUNNEL"),
        new("TUNLS", "TUNNEL"),
        new("TUNNELS", "TUNNEL"),
        new("TUNNL", "TUNNEL"),
        new("TRNPK", "TURNPIKE"),
        new("TURNPK", "TURNPIKE"),
        new("UN", "UNION"),
        new("VALLY", "VALLEY"),
        new("VLLY", "VALLEY"),
        new("VLY", "VALLEY"),
        new("VLYS", "VALLEYS"),
        new("VDCT", "VIADUCT"),
        new("VIA", "VIADUCT"),
        new("VIADCT", "VIADUCT"),
        new("VW", "VIEW"),
        new("VWS", "VIEWS"),
        new("VILL", "VILLAGE"),
        new("VILLAG", "VILLAGE"),
        new("VILLG", "VILLAGE"),
        new("VILLIAGE", "VILLAGE"),
        new("VLG", "VILLAGE"),
        new("VLGS", "VILLAGES"),
        new("VL", "VILLE"),
        new("VIS", "VISTA"),
        new("VIST", "VISTA"),
        new("VST", "VISTA"),
        new("VSTA", "VISTA"),
        new("WY", "WAY"),
        new("WLS", "WELLS"),
        new("ALLEE", "ALY"),
        new("ALLEY", "ALY"),
        new("ALLY", "ALY"),
        new("ANEX", "ANX"),
        new("ANNEX", "ANX"),
        new("ANNX", "ANX"),
        new("ARCADE", "ARC"),
        new("AV", "AVE"),
        new("AVEN", "AVE"),
        new("AVENU", "AVE"),
        new("AVENUE", "AVE"),
        new("AVN", "AVE"),
        new("AVNUE", "AVE"),
        new("BAYOO", "BYU"),
        new("BAYOU", "BYU"),
        new("BEACH", "BCH"),
        new("BEND", "BND"),
        new("BLUF", "BLF"),
        new("BLUFF", "BLF"),
        new("BLUFFS", "BLFS"),
        new("BOT", "BTM"),
        new("BOTTM", "BTM"),
        new("BOTTOM", "BTM"),
        new("BOUL", "BLVD"),
        new("BOULEVARD", "BLVD"),
        new("BOULV", "BLVD"),
        new("BRNCH", "BR"),
        new("BRANCH", "BR"),
        new("BRDGE", "BRG"),
        new("BRIDGE", "BRG"),
        new("BROOK", "BRK"),
        new("BROOKS", "BRKS"),
        new("BURG", "BG"),
        new("BURGS", "BGS"),
        new("BYPA", "BYP"),
        new("BYPAS", "BYP"),
        new("BYPASS", "BYP"),
        new("BYPS", "BYP"),
        new("CAMP", "CP"),
        new("CMP", "CP"),
        new("CANYN", "CYN"),
        new("CANYON", "CYN"),
        new("CNYN", "CYN"),
        new("CAPE", "CPE"),
        new("CAUSEWAY", "CSWY"),
        new("CAUSWA", "CSWY"),
        new("CEN", "CTR"),
        new("CENT", "CTR"),
        new("CENTER", "CTR"),
        new("CENTR", "CTR"),
        new("CENTRE", "CTR"),
        new("CNTER", "CTR"),
        new("CNTR", "CTR"),
        new("CENTERS", "CTRS"),
        new("CIRC", "CIR"),
        new("CIRCL", "CIR"),
        new("CIRCLE", "CIR"),
        new("CRCL", "CIR"),
        new("CRCLE", "CIR"),
        new("CIRCLES", "CIRS"),
        new("CLIFF", "CLF"),
        new("CLIFFS", "CLFS"),
        new("CLUB", "CLB"),
        new("COMMON", "CMN"),
        new("COMMONS", "CMNS"),
        new("CORNER", "COR"),
        new("CORNERS", "CORS"),
        new("COURSE", "CRSE"),
        new("COURT", "CT"),
        new("COURTS", "CTS"),
        new("COVE", "CV"),
        new("COVES", "CVS"),
        new("CREEK", "CRK"),
        new("CRESCENT", "CRES"),
        new("CRSENT", "CRES"),
        new("CRSNT", "CRES"),
        new("CREST", "CRST"),
        new("CROSSING", "XING"),
        new("CRSSNG", "XING"),
        new("CROSSROAD", "XRD"),
        new("CROSSROADS", "XRDS"),
        new("CURVE", "CURV"),
        new("DALE", "DL"),
        new("DAM", "DM"),
        new("DIV", "DV"),
        new("DIVIDE", "DV"),
        new("DVD", "DV"),
        new("DRIV", "DR"),
        new("DRIVE", "DR"),
        new("DRV", "DR"),
        new("DRIVES", "DRS"),
        new("ESTATE", "EST"),
        new("ESTATES", "ESTS"),
        new("EXP", "EXPY"),
        new("EXPR", "EXPY"),
        new("EXPRESS", "EXPY"),
        new("EXPRESSWAY", "EXPY"),
        new("EXPW", "EXPY"),
        new("EXTENSION", "EXT"),
        new("EXTN", "EXT"),
        new("EXTNSN", "EXT"),
        new("FALLS", "FLS"),
        new("FERRY", "FRY"),
        new("FRRY", "FRY"),
        new("FIELD", "FLD"),
        new("FIELDS", "FLDS"),
        new("FLAT", "FLT"),
        new("FLATS", "FLTS"),
        new("FORD", "FRD"),
        new("FORDS", "FRDS"),
        new("FOREST", "FRST"),
        new("FORESTS", "FRST"),
        new("FORG", "FRG"),
        new("FORGE", "FRG"),
        new("FORGES", "FRGS"),
        new("FORK", "FRK"),
        new("FORKS", "FRKS"),
        new("FORT", "FT"),
        new("FRT", "FT"),
        new("FREEWAY", "FWY"),
        new("FREEWY", "FWY"),
        new("FRWAY", "FWY"),
        new("FRWY", "FWY"),
        new("GARDEN", "GDN"),
        new("GARDN", "GDN"),
        new("GRDEN", "GDN"),
        new("GRDN", "GDN"),
        new("GARDENS", "GDNS"),
        new("GRDNS", "GDNS"),
        new("GATEWAY", "GTWY"),
        new("GATEWY", "GTWY"),
        new("GATWAY", "GTWY"),
        new("GTWAY", "GTWY"),
        new("GLEN", "GLN"),
        new("GLENS", "GLNS"),
        new("GREEN", "GRN"),
        new("GREENS", "GRNS"),
        new("GROV", "GRV"),
        new("GROVE", "GRV"),
        new("GROVES", "GRVS"),
        new("HARB", "HBR"),
        new("HARBOR", "HBR"),
        new("HARBR", "HBR"),
        new("HRBOR", "HBR"),
        new("HARBORS", "HBRS"),
        new("HAVEN", "HVN"),
        new("HT", "HTS"),
        new("HIGHWAY", "HWY"),
        new("HIGHWY", "HWY"),
        new("HIWAY", "HWY"),
        new("HIWY", "HWY"),
        new("HWAY", "HWY"),
        new("HILL", "HL"),
        new("HILLS", "HLS"),
        new("HLLW", "HOLW"),
        new("HOLLOW", "HOLW"),
        new("HOLLOWS", "HOLW"),
        new("HOLWS", "HOLW"),
        new("ISLAND", "IS"),
        new("ISLND", "IS"),
        new("ISLANDS", "ISS"),
        new("ISLNDS", "ISS"),
        new("ISLES", "ISLE"),
        new("JCTION", "JCT"),
        new("JCTN", "JCT"),
        new("JUNCTION", "JCT"),
        new("JUNCTN", "JCT"),
        new("JUNCTON", "JCT"),
        new("JCTNS", "JCTS"),
        new("JUNCTIONS", "JCTS"),
        new("KEY", "KY"),
        new("KEYS", "KYS"),
        new("KNOL", "KNL"),
        new("KNOLL", "KNL"),
        new("KNOLLS", "KNLS"),
        new("LAKE", "LK"),
        new("LAKES", "LKS"),
        new("LANDING", "LNDG"),
        new("LNDNG", "LNDG"),
        new("LANE", "LN"),
        new("LIGHT", "LGT"),
        new("LIGHTS", "LGTS"),
        new("LOAF", "LF"),
        new("LOCK", "LCK"),
        new("LOCKS", "LCKS"),
        new("LDGE", "LDG"),
        new("LODG", "LDG"),
        new("LODGE", "LDG"),
        new("LOOPS", "LOOP"),
        new("MANOR", "MNR"),
        new("MANORS", "MNRS"),
        new("MEADOW", "MDW"),
        new("MDW", "MDWS"),
        new("MEADOWS", "MDWS"),
        new("MEDOWS", "MDWS"),
        new("MILL", "ML"),
        new("MILLS", "MLS"),
        new("MISSN", "MSN"),
        new("MSSN", "MSN"),
        new("MOTORWAY", "MTWY"),
        new("MNT", "MT"),
        new("MOUNT", "MT"),
        new("MNTAIN", "MTN"),
        new("MNTN", "MTN"),
        new("MOUNTAIN", "MTN"),
        new("MOUNTIN", "MTN"),
        new("MTIN", "MTN"),
        new("MNTNS", "MTNS"),
        new("MOUNTAINS", "MTNS"),
        new("NECK", "NCK"),
        new("ORCHARD", "ORCH"),
        new("ORCHRD", "ORCH"),
        new("OVL", "OVAL"),
        new("OVERPASS", "OPAS"),
        new("PRK", "PARK"),
        new("PARKS", "PARK"),
        new("PARKWAY", "PKWY"),
        new("PARKWY", "PKWY"),
        new("PKWAY", "PKWY"),
        new("PKY", "PKWY"),
        new("PARKWAYS", "PKWY"),
        new("PKWYS", "PKWY"),
        new("PASSAGE", "PSGE"),
        new("PATHS", "PATH"),
        new("PIKES", "PIKE"),
        new("PINE", "PNE"),
        new("PINES", "PNES"),
        new("PLAIN", "PLN"),
        new("PLAINS", "PLNS"),
        new("PLAZA", "PLZ"),
        new("PLZA", "PLZ"),
        new("POINT", "PT"),
        new("POINTS", "PTS"),
        new("PORT", "PRT"),
        new("PORTS", "PRTS"),
        new("PRAIRIE", "PR"),
        new("PRR", "PR"),
        new("RAD", "RADL"),
        new("RADIAL", "RADL"),
        new("RADIEL", "RADL"),
        new("RANCH", "RNCH"),
        new("RANCHES", "RNCH"),
        new("RNCHS", "RNCH"),
        new("RAPID", "RPD"),
        new("RAPIDS", "RPDS"),
        new("REST", "RST"),
        new("RDGE", "RDG"),
        new("RIDGE", "RDG"),
        new("RIDGES", "RDGS"),
        new("RIVER", "RIV"),
        new("RVR", "RIV"),
        new("RIVR", "RIV"),
        new("ROAD", "RD"),
        new("ROADS", "RDS"),
        new("ROUTE", "RTE"),
        new("SHOAL", "SHL"),
        new("SHOALS", "SHLS"),
        new("SHOAR", "SHR"),
        new("SHORE", "SHR"),
        new("SHOARS", "SHRS"),
        new("SHORES", "SHRS"),
        new("SKYWAY", "SKWY"),
        new("SPNG", "SPG"),
        new("SPRING", "SPG"),
        new("SPRNG", "SPG"),
        new("SPNGS", "SPGS"),
        new("SPRINGS", "SPGS"),
        new("SPRNGS", "SPGS"),
        new("SPURS", "SPUR"),
        new("SQR", "SQ"),
        new("SQRE", "SQ"),
        new("SQU", "SQ"),
        new("SQUARE", "SQ"),
        new("SQRS", "SQS"),
        new("SQUARES", "SQS"),
        new("STATION", "STA"),
        new("STATN", "STA"),
        new("STN", "STA"),
        new("STRAV", "STRA"),
        new("STRAVEN", "STRA"),
        new("STRAVENUE", "STRA"),
        new("STRAVN", "STRA"),
        new("STRVN", "STRA"),
        new("STRVNUE", "STRA"),
        new("STREAM", "STRM"),
        new("STREME", "STRM"),
        new("STREET", "ST"),
        new("STRT", "ST"),
        new("STR", "ST"),
        new("STREETS", "STS"),
        new("SUMIT", "SMT"),
        new("SUMITT", "SMT"),
        new("SUMMIT", "SMT"),
        new("TERR", "TER"),
        new("TERRACE", "TER"),
        new("THROUGHWAY", "TRWY"),
        new("TRACE", "TRCE"),
        new("TRACES", "TRCE"),
        new("TRACK", "TRAK"),
        new("TRACKS", "TRAK"),
        new("TRK", "TRAK"),
        new("TRKS", "TRAK"),
        new("TRAFFICWAY", "TRFY"),
        new("TRAIL", "TRL"),
        new("TRAILS", "TRL"),
        new("TRLS", "TRL"),
        new("TRAILER", "TRLR"),
        new("TRLRS", "TRLR"),
        new("TUNEL", "TUNL"),
        new("TUNLS", "TUNL"),
        new("TUNNEL", "TUNL"),
        new("TUNNELS", "TUNL"),
        new("TUNNL", "TUNL"),
        new("TRNPK", "TPKE"),
        new("TURNPIKE", "TPKE"),
        new("TURNPK", "TPKE"),
        new("UNDERPASS", "UPAS"),
        new("UNION", "UN"),
        new("UNIONS", "UNS"),
        new("VALLEY", "VLY"),
        new("VALLY", "VLY"),
        new("VLLY", "VLY"),
        new("VALLEYS", "VLYS"),
        new("VDCT", "VIA"),
        new("VIADCT", "VIA"),
        new("VIADUCT", "VIA"),
        new("VIEW", "VW"),
        new("VIEWS", "VWS"),
        new("VILL", "VLG"),
        new("VILLAG", "VLG"),
        new("VILLAGE", "VLG"),
        new("VILLG", "VLG"),
        new("VILLIAGE", "VLG"),
        new("VILLAGES", "VLGS"),
        new("VILLE", "VL"),
        new("VIST", "VIS"),
        new("VISTA", "VIS"),
        new("VST", "VIS"),
        new("VSTA", "VIS"),
        new("WALKS", "WALK"),
        new("WY", "WAY"),
        new("WELL", "WL"),
        new("WELLS", "WLS")
    };
}
