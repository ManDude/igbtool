#include <cstdlib>
#include <cstdio>
#include <cstring>
#include <cerrno>

#include <vector>
#include <string>

#include "igb.h"

#include "common/assert.h"
#include "common/types.h"
#include "common/log.h"
#include "common/err.h"
#include "common/math.h"

int main(int argc, char* argv[]) {
  igb_init_globals();
  lg::initialize();

  if (argc < 2) {
    fmt::print("USAGE:  igbtool <igb>\n");
    return IGB_EXIT;
  }

  FILE* fp = fopen(argv[1], "rb");
  if (!fp) {
    lg::error("Failed to open IGB: {}", strerror(errno));
    return IGB_FILE_ERROR;
  }

  fseek(fp, 0, SEEK_END);
  auto size = ftell(fp);
  std::vector<u8> data(size);
  fseek(fp, 0, SEEK_SET);
  fread(data.data(), sizeof(u8), size, fp);
  fclose(fp);

  IgbFile igb(data);

  if (argc < 3) {
    char mystr[4];
    int masterrun = true;
    while (masterrun) {
      fmt::print("Please enter command: ");
      fgets(mystr, 4, stdin);
      switch (mystr[0]) {
        case 't':
          fmt::print(igb.print_types());
          break;
        case 's':
          fmt::print(igb.print_structs());
          break;
        case 'r':
          fmt::print(igb.print_refs());
          break;
        case 'o':
          fmt::print(igb.print_objects());
          break;
        case 'd':
          fmt::print(igb.print_data());
          break;
        case 'm':
          fmt::print(igb.print_misc());
          break;
        case 'f':
          fmt::print(igb.print_types());
          fmt::print(igb.print_structs());
          fmt::print(igb.print_refs());
          fmt::print(igb.print_objects());
          fmt::print(igb.print_data());
          fmt::print(igb.print_misc());
          fmt::print(igb.print_graph());
          break;
        case 'g':
          fmt::print(igb.print_graph());
          break;
        case 'i':
          igb.RipImages();
          break;
        case 'x':
          masterrun = false;
          break;
        default:
          fmt::print("Unsupported command.\n");
      }
    }
  } else {
    std::string out;
    out += igb.print_types();
    out += igb.print_structs();
    out += igb.print_refs();
    out += igb.print_objects();
    out += igb.print_data();
    out += igb.print_misc();
    out += igb.print_graph();
    FILE* fp = fopen(argv[2], "w");
    if (!fp) {
      lg::error("Failed to open output file: {}\n", strerror(errno));
      return IGB_FILE_ERROR;
    }
    fprintf(fp, "%s\n", out.c_str());
    fclose(fp);
    lg::info("Done!");
  }

  return IGB_OK;
}
