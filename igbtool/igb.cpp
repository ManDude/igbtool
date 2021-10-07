#include "igb.h"

#include "common/assert.h"
#include "common/err.h"
#include "common/math.h"
#include "common/log.h"
#include "common/print_float.h"

#include <vector>
#include <string>
#include <unordered_set>
#include <stdexcept>
#include <exception>
#include <algorithm>

std::string IgbTypeInfo::print(const u8* data) const {
  return fmt::format("{}: ", name) + print_func(*this, data);
}
std::string IgbTypeInfo::print_no_name(const u8* data) const {
  return print_func(*this, data);
}

std::vector<std::string> IgbTypeInfo::parse_format() const {
  std::vector<std::string> out;

  auto start = default_format.empty() ? std::string::npos : 0;
  while (start != std::string::npos) {
    auto end = default_format.find(' ', start);
    out.push_back(default_format.substr(start, end - start));
    start = default_format.find_first_not_of(' ', end);
  }
  return out;
}

std::string type_info_default_func(const IgbTypeInfo& obj, const u8* data) {
  std::string out = "";

  int off = 0;
  auto args = obj.parse_format();
  bool first = true;
  for (auto& arg : args) {
    if (first) {
      first = false;
    } else {
      out += " ";
    }
    if (arg == "bool") {
      out += fmt::format("{}", *(bool*)(data + off));
      off += sizeof(bool);
    } else if (arg == "s8") {
      out += fmt::format("{:<4}", *(s8*)(data + off));
      off += sizeof(s8);
    } else if (arg == "u8") {
      out += fmt::format("0x{:02X}", *(u8*)(data + off));
      off += sizeof(u8);
    } else if (arg == "s16") {
      out += fmt::format("{:<7}", *(s16*)(data + off));
      off += sizeof(s16);
    } else if (arg == "u16") {
      out += fmt::format("0x{:04X}", *(u16*)(data + off));
      off += sizeof(u16);
    } else if (arg == "s32") {
      out += fmt::format("{:<11}", *(s32*)(data + off));
      off += sizeof(s32);
    } else if (arg == "u32") {
      out += fmt::format("0x{:08X}", *(u32*)(data + off));
      off += sizeof(u32);
    } else if (arg == "s64") {
      out += fmt::format("{:<16}", *(s64*)(data + off));
      off += sizeof(s64);
    } else if (arg == "u64") {
      out += fmt::format("0x{:016X}", *(u64*)(data + off));
      off += sizeof(u64);
    } else if (arg == "ref") {
      out += fmt::format("{:04X}", *(u32*)(data + off));
      off += sizeof(u32);
    } else if (arg == "float") {
      off = align(off, 4);
      out += fmt::format("{:14}", float_to_string(*(float*)(data + off)));
      off += sizeof(float);
    } else if (arg == "double") {
      out += fmt::format("{}", *(double*)(data + off));
      off += sizeof(double);
    } else {
      lg::error("Unknown format {} for type info", arg);
      out += fmt::format("[unknown fmt arg {}]", arg);
    }
  }
  return out;
}

void IgbTypeInfo::guess_size() {
  size = -4;
  auto args = parse_format();
  bool first = true;
  for (auto& arg : args) {
    if (first) {
      size = 0;
      first = false;
    }
    if (arg == "bool") {
      size += sizeof(bool);
    } else if (arg == "s8") {
      size += sizeof(s8);
    } else if (arg == "u8") {
      size += sizeof(u8);
    } else if (arg == "s16") {
      size += sizeof(s16);
    } else if (arg == "u16") {
      size += sizeof(u16);
    } else if (arg == "s32") {
      size += sizeof(s32);
    } else if (arg == "u32") {
      size += sizeof(u32);
    } else if (arg == "s64") {
      size += sizeof(s64);
    } else if (arg == "u64") {
      size += sizeof(u64);
    } else if (arg == "ref") {
      size += sizeof(u32);
    } else if (arg == "float") {
      size = align(size, 4);
      size += sizeof(float);
    } else if (arg == "double") {
      size += sizeof(double);
    } else {
      lg::warn("Unknown format {} for type info", arg);
    }
  }
}

std::string type_info_string_func(const IgbTypeInfo& obj, const u8* data) {
  s32 sz = *(s32*)data;
  return fmt::format("\"{}\"", sz == 0 ? "" : std::string((const char*)(data + 4)));
}

std::vector<IgbTypeInfo> type_infos = {
    IgbTypeInfo("igBoolMetaField", "bool"),
    IgbTypeInfo("igCharMetaField", "s8"),
    IgbTypeInfo("igUnsignedCharMetaField", "u8"),
    IgbTypeInfo("igShortMetaField", "s16"),
    IgbTypeInfo("igUnsignedShortMetaField", "u16"),
    IgbTypeInfo("igIntMetaField", "s32"),
    IgbTypeInfo("igEnumMetaField", "s32"),
    IgbTypeInfo("igUnsignedIntMetaField", "u32"),
    IgbTypeInfo("igLongMetaField", "s64"),
    IgbTypeInfo("igUnsignedLongMetaField", "u64"),
    IgbTypeInfo("igFloatMetaField", "float"),
    IgbTypeInfo("igDoubleMetaField", "double"),
    IgbTypeInfo("igRawRefMetaField", "ref"),
    IgbTypeInfo("igMemoryRefMetaField", "ref"),
    IgbTypeInfo("igObjectRefMetaField", "ref"),
    IgbTypeInfo("igVec2fMetaField", "float float"),
    IgbTypeInfo("igVec3fMetaField", "float float float"),
    IgbTypeInfo("igVec4fMetaField", "float float float float"),
    IgbTypeInfo("igMatrix44fMetaField", "float float float float float float float float float float float float float float float float"),
    IgbTypeInfo("CNKLetterDataMetaField", "s32 float float float float float float float"),
    IgbTypeInfo("RestartPointDataMetaField", "float float float float float float float s32"),
    IgbTypeInfo("RankingNodeMetaField", "float float float float float float float float float float float s32"),
    IgbTypeInfo("MagGravNodeMetaField", "float float float float float float"),
    IgbTypeInfo("CrystalDataMetaField", "float float float float float float float s8"),
    IgbTypeInfo("ConveyorDataMetaField", "s8 float float float float float float float float float float float float float float s8"),
    IgbTypeInfo("CrateDataMetaField", "s32 float float float float float float float"),
    IgbTypeInfo("CtfFlagDataMetaField", "s32 float float float float float float float"),
    IgbTypeInfo("CtfStartPositionDataMetaField", "float float float float float float float"),
    IgbTypeInfo("LanStartGameMetaField", "float float float float float float float"),
    IgbTypeInfo("LanTeamSelectMetaField", "float float float float float float float s32"),
    IgbTypeInfo("igStringMetaField", type_info_string_func),
    IgbTypeInfo("igStructMetaField", "u32", IgbTypeInfoFlags::DYNAMIC)

};

std::unordered_map<std::string, const IgbTypeInfo*> type_info_map;

IgbFile::IgbFile(const std::vector<u8>& data) {
  size_t seek = 0;
  size_t oldseek = 0;

  IgbHeader* header = (IgbHeader*)&data.at(0);
  version = header->version;

  // FIFTH SECTION (4) - STATIC SYMBOLS
  {
    seek = oldseek = sizeof(IgbHeader);
    // type headers
    for (int i = 0; i < header->sections[4].count; ++i) {
      auto& new_type = types.emplace_back();
      new_type.name_length = *(u32*)&data.at(seek + 0);
      auto b = *(u32*)&data.at(seek + 4);
      ASSERT(b == (u32) true || b == (u32) false);
      new_type.u_bool = (bool)b;
      // new_type.u_val = *(u32*)&data.at(seek + 8);
      ASSERT(*(u32*)&data.at(seek + 8) == 0);
      new_type.size = -4;
      seek += 12;
    }
    // type names
    for (int i = 0; i < header->sections[4].count; ++i) {
      auto& type = types[i];
      auto name = std::string((const char*)&data.at(seek));
      ASSERT(name.size() + 1 == type.name_length);
      seek += type.name_length;
      type.name = name;

      // copy info
      if (type_info_map.find(name) != type_info_map.end()) {
        auto info = type_info_map.at(name);
        type.size = info->get_size();
        type.dynamic = info->dynamic;
      }

      auto x = name.find("ArrayMetaField");
      type.is_array = x != std::string::npos && name.substr(x) == "ArrayMetaField";
      if (type.is_array) {
        name.replace(x, strlen("ArrayMetaField"), "MetaField");
        auto elt_type = get_type_by_name(name);
        ASSERT(elt_type != nullptr);
        type.elt_type = elt_type;
      }
    }
    ASSERT(seek - oldseek == header->sections[4].size);
    // shader subsection header
    oldseek = seek;
    u32 shader_sec_size = *(u32*)&data.at(seek);
    shader_val = *(u32*)&data.at(seek + 4);
    u32 shader_sym_amt = *(u32*)&data.at(seek + 8);
    u32* shader_sym_name_lengths = (u32*)&data.at(seek + 12);
    seek += 12 + 4 * shader_sym_amt;
    // names
    for (int i = 0; i < shader_sym_amt; ++i) {
      auto name = std::string((const char*)&data.at(seek));
      ASSERT(name.size() + 1 == shader_sym_name_lengths[i]);
      seek += shader_sym_name_lengths[i];
      auto& new_sym = shader_symbols.emplace_back();
      new_sym.name = name;
    }
    ASSERT(seek - oldseek == shader_sec_size);
  }

  // SECOND SECTION (1) - STRUCT DEFS
  {
    oldseek = seek;
    IgbStructDef* struct_defs = (IgbStructDef*)&data.at(seek);
    seek += sizeof(IgbStructDef) * header->sections[1].count;
    for (int i = 0; i < header->sections[1].count; ++i) {
      auto def = struct_defs[i];
      auto name = std::string((const char*)&data.at(seek));
      ASSERT(align(name.size() + 1, 2) == def.name_length);
      seek += def.name_length;
      auto& new_struct = structs.emplace_back();
      new_struct.id = i;
      new_struct.name = name;
      new_struct.is_master = def.is_master;
      // new_struct.u_val1 = def.u_val1;
      ASSERT(def.u_val1 == 0);
      new_struct.u_val2 = def.u_val2;
      new_struct.parent = def.parent;
      new_struct.field_count = def.field_count;
      for (int ii = 0; ii < def.field_count; ++ii) {
        auto& field = new_struct.fields.emplace_back();
        field.type = &types[*(u16*)&data.at(seek)];
        field.unk = *(u16*)&data.at(seek + 2);
        field.size = *(u16*)&data.at(seek + 4);
        auto type = &types[*(u16*)&data.at(seek)];
        if (!type->is_array && !type->dynamic) {
          if (type->size == -4) {
            type->size = field.size;
          } else if (type->size != field.size) {
            lg::error("type size disagreement on {} (was {} vs. {})", type->name, type->size, field.size);
            ASSERT(false);
          }
        }
        seek += 6;
      }
    }
    ASSERT(seek - oldseek == header->sections[1].size);
  }

  // FIRST SECTION (0) - IGB REFS
  {
    oldseek = seek;
    for (int i = 0; i < header->sections[0].count; ++i) {
      IgbRef* ref = (IgbRef*)&data.at(seek);
      ASSERT(ref->unk == 0);
      if (ref->type == 3) {
        IgbObjectRef* objref = (IgbObjectRef*)ref;
        auto& newobj = objs.emplace_back();
        newobj.obj_struct = &structs[objref->struct_id];
        newobj.ref_idx = i;
        ref_idx.emplace_back(IgbRefKind::OBJECT, objs.size() - 1);
      } else if (ref->type == 4) {
        IgbStaticRef* dataref = (IgbStaticRef*)ref;
        auto& newdata = static_data.emplace_back();
        newdata.unk1 = dataref->unk;
        newdata.unk2 = dataref->unk2;
        newdata.unk3 = dataref->unk3;
        newdata.type = &types[dataref->data_type];
        newdata.data.resize(dataref->data_size);
        newdata.ref_idx = i;
        ref_idx.emplace_back(IgbRefKind::DATA, static_data.size() - 1);
      } else {
        lg::error("unknown ref type {}", ref->type);
        ref_idx.emplace_back(IgbRefKind::UNKNOWN, -1);
        ASSERT(false);
      }
      seek += ref->size;
    }
    ASSERT(seek - oldseek == header->sections[0].size);
  }

  if (version == 0x80000005) {
    top_object = *(u32*)&data.at(seek);
    seek += 4;
  }

  // THIRD SECTION (2) - OBJECTS
  {
    oldseek = seek;
    for (int i = 0; i < header->sections[2].count; ++i) {
      u32 struct_id = *(u32*)&data.at(seek);
      u32 size = *(u32*)&data.at(seek + 4);

      auto& newobj = objs.at(i);
      ASSERT(struct_id == newobj.obj_struct->id);
      newobj.data.resize(size - 8);
      memcpy(newobj.data.data(), &data.at(seek + 8), size - 8);

      seek += size;
    }
    ASSERT(seek - oldseek == header->sections[2].size);
  }

  // FOURTH SECTION (3) - STATIC DATA
  {
    oldseek = seek;
    for (int i = 0; i < header->sections[3].count; ++i) {
      auto& mem = static_data.at(i);
      auto data_sz = mem.data.size();
      memcpy(mem.data.data(), &data.at(seek), data_sz);
      seek = oldseek + align(seek - oldseek + data_sz, 4);
    }
    ASSERT(seek - oldseek == header->sections[3].size);
  }

  if (version == 0x80000004) {
    top_object = *(u32*)&data.at(seek);
    seek += 4;
  }

  ASSERT(header->sections[0].count == ref_idx.size());
  ASSERT(header->sections[1].count == structs.size());
  ASSERT(header->sections[2].count == objs.size());
  ASSERT(header->sections[3].count == static_data.size());
  ASSERT(header->sections[4].count == types.size());
  ASSERT(seek == data.size());
}

namespace {
std::string line() {
  return "----------------------------------------\n";
}
std::string linesmall() {
  return "---------------\n";
}
}  // namespace

std::string IgbFile::print_types() {
  std::string out = line();
  for (int i = 0; i < types.size(); ++i) {
    auto& type = types[i];
    out += fmt::format("{:08X}: {:32} (b: {})", i, type.name, type.u_bool);
    if (type.is_array) {
      out += " (array)";
    }
    if ((!type.is_array && type_info_map.find(type.name) == type_info_map.end()) ||
        (type.is_array && type_info_map.find(type.elt_type->name) == type_info_map.end())) {
      lg::warn("Unknown type {}", type.name);
      out += " [UNKNOWN TYPE!]";
    }
    out += "\n";
  }
  out += linesmall();
  for (int i = 0; i < shader_symbols.size(); ++i) {
    auto& sym = shader_symbols[i];
    out += fmt::format("{:08X}: {}\n", i, sym.name);
  }
  return out;
}

std::string IgbFile::print_structs() {
  std::string out = line();
  out += fmt::format("Structs: {}\n", structs.size());
  out += linesmall();
  for (int i = 0; i < structs.size(); ++i) {
    auto& cls = structs.at(i);
    out += fmt::format("[{:02X}] {}", i, cls.name);
    if (cls.is_master)
      out += fmt::format(" (master)");
    out += "\n";
    // out += fmt::format("    Unknown: {}\n", cls.u_val1);
    if (cls.parent != -1) {
      out += fmt::format("    Parent: {}\n", structs[cls.parent].name);
    }
    out += fmt::format("    Unknown: {}\n", cls.u_val2);
    out += fmt::format("    Fields: {}\n", cls.field_count);
    for (int j = 0; j < cls.field_count; ++j) {
      auto& field = cls.fields.at(j);
      out += fmt::format("        {} ({} bytes) [??: {}]\n", field.type->name, field.size, field.unk);
    }
  }
  return out;
}

std::string IgbFile::print_refs() {
  std::string out = line();
  out += fmt::format("Total Count: {}\n", ref_idx.size());
  out += linesmall();
  out += fmt::format("Object Count: {}\n", objs.size());
  for (int i = 0; i < objs.size(); ++i) {
    auto& obj = objs[i];
    out += fmt::format("[Object {:04X}] {}\n", obj.ref_idx, obj.obj_struct->name);
  }
  out += linesmall();
  out += fmt::format("Static Data Count: {}\n", static_data.size());
  for (int i = 0; i < static_data.size(); ++i) {
    auto& mem = static_data[i];
    out += fmt::format("[Data {:04X}] Size: {:>5X}  bool: {}  Special: {:2}  Type: {}\n", mem.ref_idx, mem.data.size(), mem.unk2 & 0xFF,
                       mem.unk3, mem.type->name);
  }
  return out;
}

std::string IgbFile::print_misc() {
  std::string out = line();
  out += fmt::format("Shader Val: {}\nTop-level Object: {:04X}\n", shader_val, top_object);
  return out;
}

int IgbFile::get_type_size(const IgbType* type, const u8* data) const {
  /*
  if (type->dynamic) {
    lg::error("type {} is dynamic, can't retrieve size", type->name);
    throw std::invalid_argument(fmt::format("type {} is dynamic, can't retrieve size", type->name));
  }*/
  return type->name == "igStringMetaField" ? type->size + *(u32*)data : type->size;
}

const IgbType* IgbFile::get_type_by_name(const std::string& name) const {
  for (auto& type : types) {
    if (type.name == name)
      return &type;
  }
  return nullptr;
}

int IgbFile::get_field_size(const IgbStructField& field, const u8* data) const {
  if (field.type->is_array) {
    /* dont touch until we run into issues
    auto name = std::string(field.type->name);
    name.replace(name.find("ArrayMetaField"), strlen("ArrayMetaField"), "MetaField");
    auto elt_type = get_type_by_name(name);
    ASSERT(elt_type != nullptr);
    int data_off = 0;
    while (data_off < field.size) {
      data_off += get_type_size(elt_type, data + data_off);
    }
    return data_off;*/
    return field.size;
  } else if (field.type->dynamic) {
    return field.size;
  } else {
    return get_type_size(field.type, data);
  }
}

std::string IgbFile::print_type_data(const IgbType* type, const u8* data, bool noname) const {
  auto& fv = type_info_map.find(type->name);
  if (fv != type_info_map.end()) {
    auto& field_type_info = *fv->second;
    return noname ? field_type_info.print_no_name(data) : field_type_info.print(data);
  } else {
    lg::warn("Unknown field type {}", type->name);
    return fmt::format("UNKNOWN FIELD TYPE {}", type->name);
  }
}

std::string IgbFile::print_field_data(const IgbStructField& field, const u8* data) const {
  if (field.type->is_array) {
    auto& fv = type_info_map.find(field.type->elt_type->name);
    std::string out = fmt::format("{}: [\n", field.type->name);
    int off = 0;
    while (off < field.size) {
      if (off > 0) {
        out += ",\n";
      }
      out += "        " + print_type_data(field.type->elt_type, data + off, true);
      off += get_type_size(field.type->elt_type, data + off);
    }
    out += "\n    ]";
    return out;
  } else {
    return print_type_data(field.type, data);
  }
}

std::string IgbFile::print_objects() {
  std::string out = line();
  for (int i = 0; i < objs.size(); ++i) {
    auto& obj = objs[i];
    out += fmt::format("[Object {:04X}] Size: {:04X} Struct: {}\n", obj.ref_idx, obj.data.size(), obj.obj_struct->name);
    int data_off = 0;
    for (int j = 0; j < obj.obj_struct->field_count; ++j) {
      auto& field = obj.obj_struct->fields[j];
      auto type = field.type;
      auto data = &obj.data.at(data_off);
      out += "    ";
      out += print_field_data(field, data);
      out += "\n";
      data_off = align(data_off + get_field_size(obj.obj_struct->fields[j], data), 4);
    }
    ASSERT(data_off == obj.data.size());
  }
  return out;
}

std::string IgbFile::print_data() {
  std::string out = line();
  out += fmt::format("STATIC DATA COUNT: {}\n", static_data.size());
  out += linesmall();
  for (int i = 0; i < static_data.size(); ++i) {
    auto& mem = static_data[i];
    out += "\n";
    out += fmt::format("[Static Data {:04X}] Size: {:04X} ({})", mem.ref_idx, mem.data.size(), mem.type->name);
    if (type_info_map.find(mem.type->name) == type_info_map.end()) {
      lg::error("Unknown static data type {}", mem.type->name);
      out += " [UNKNOWN]\n";
      continue;
    }
    out += "\n";
    int data_off = 0;
    auto type = mem.type;
    while (data_off < mem.data.size()) {
      auto data = &mem.data.at(data_off);
      out += "    ";
      out += print_type_data(type, data, true);
      out += "\n";
      data_off = align(data_off + get_type_size(type, data), 4);
    }
    ASSERT(data_off == align(mem.data.size(), 4));
  }
  return out;
}

void IgbFile::print_graph_child(std::string& str,
                                std::vector<int>& id_map,
                                std::vector<int>& id_stack,
                                int id,
                                int level,
                                std::vector<bool>& child_bits,
                                bool print_child,
                                bool recursive_warn) {
  // ASSERT(level < 64);
  auto kind = get_ref_kind(id);
  if (kind == IgbRefKind::NONE) {
    return;
  }

  str += "    ";
  for (auto c : child_bits) {
    str += c ? "|   " : "    ";
  }
  str += fmt::format("+-- {:04X} ", id);

  bool recursive = std::find(id_stack.cbegin(), id_stack.cend(), id) != id_stack.cend();
  if (recursive) {
    lg::warn("Infinite recursion on {:04X} detected", id);
  }

  id_stack.push_back(id);
  child_bits.push_back(print_child);
  id_map.at(id)++;

  switch (kind) {
    default:
    case IgbRefKind::NONE:
      lg::error("invalid ref when printing graph");
      break;
    case IgbRefKind::DATA: {
      auto mem = get_data_from_ref(id);
      str += fmt::format("(list {})", mem->type->name);
      if (!recursive_warn) {
        str += "\n";
        if (mem->type->name == "igObjectRefMetaField" || mem->type->name == "igMemoryRefMetaField") {
          std::vector<u32> ids;
          int off = 0;
          while (off < mem->data.size()) {
            ids.push_back(*(u32*)&mem->data.at(off));
            off += 4;
          }
          for (int i = 0; i < ids.size(); ++i) {
            print_graph_child(str, id_map, id_stack, ids.at(i), level + 1, child_bits, i < ids.size() - 1, recursive);
          }
        }
      } else {
        str += fmt::format(" (recursive)\n");
        lg::warn("Infinite recursion on {:04X} detected", id);
      }
    } break;
    case IgbRefKind::OBJECT: {
      std::vector<u32> ids;
      auto obj = get_object_from_ref(id);
      str += fmt::format("({})", obj->obj_struct->name);
      if (!recursive_warn) {
        str += "\n";
        int off = 0;
        for (auto& field : obj->obj_struct->fields) {
          if (field.type->name == "igObjectRefMetaField" || field.type->name == "igMemoryRefMetaField") {
            ids.push_back(*(u32*)&obj->data.at(off));
          } else if (field.type->name == "igObjectRefArrayMetaField") {
            int soff = 0;
            while (soff < field.size) {
              ids.push_back(*(u32*)&obj->data.at(off + soff));
              soff += 4;
            }
          }
          off = align(off + get_field_size(field, obj->data.data() + off), 4);
        }
        for (int i = 0; i < ids.size(); ++i) {
          print_graph_child(str, id_map, id_stack, ids.at(i), level + 1, child_bits, i < ids.size() - 1, recursive);
        }
      } else {
        str += fmt::format(" (recursive)\n");
      }
    } break;
  }
  child_bits.pop_back();
  id_stack.pop_back();
}

std::string IgbFile::print_graph() {
  auto obj = get_object_from_ref(top_object);
  auto list_mem_id = *(u32*)&obj->data.at(8);
  std::vector<int> id_map(ref_idx.size());
  id_map.at(top_object)++;

  std::vector<bool> child_bits;
  std::vector<int> id_stack;
  std::string out = line();
  out += fmt::format("--- {:04X} ({})\n", top_object, obj->obj_struct->name);
  print_graph_child(out, id_map, id_stack, list_mem_id, 0, child_bits, false, false);

  bool error = false;
  for (int i = 0; i < id_map.size(); ++i) {
    if (id_map.at(i) > 1) {
      // lg::warn("ref ID {:04X} referenced {} times", i, id_map.at(i));
      // error = true;
    } else if (id_map.at(i) == 0) {
      lg::error("ref ID {:04X} not referenced", i);
      error = true;
    }
  }
  if (!error) {
    lg::info("All refs referenced correctly!");
  }

  return out;
}

IgbRefKind IgbFile::get_ref_kind(int idx) const {
  if (idx == -1)
    return IgbRefKind::NONE;
  return ref_idx.at(idx).type;
}

const IgbObject* IgbFile::get_object_from_ref(int idx) {
  if (idx == -1)
    return nullptr;
  auto& ref = ref_idx.at(idx);
  if (ref.type != IgbRefKind::OBJECT) {
    lg::error("Invalid object ref {}", idx);
    ASSERT(false);
  }
  return &objs.at(ref.index);
}

const IgbStaticData* IgbFile::get_data_from_ref(int idx) {
  if (idx == -1)
    return nullptr;
  auto& ref = ref_idx.at(idx);
  if (ref.type != IgbRefKind::DATA) {
    lg::error("Invalid data ref {}", idx);
    ASSERT(false);
  }
  return &static_data.at(ref.index);
}

void igb_init_globals() {
  for (auto& t : type_infos) {
    type_info_map.try_emplace(t.name, &t);
  }
}

void IgbFile::RipImages() {
  int img_found = 0;
  for (int i = 0; i < objs.size(); ++i) {
    if (objs[i].obj_struct->name == "igImage") {
      ++img_found;
      std::string img_fname = std::string((const char*)(objs[i].data.data() + 84));
      img_fname = img_fname.substr(img_fname.find_last_of("\\") + 1) + ".bmp";
      FILE* bmp = fopen(img_fname.c_str(), "wb");
      if (!bmp)
        continue;
      u32 w = *(u32*)&objs[i].data.at(0), h = *(u32*)&objs[i].data.at(4), r = *(u32*)&objs[i].data.at(20), g = *(u32*)&objs[i].data.at(24),
          b = *(u32*)&objs[i].data.at(28), a = *(u32*)&objs[i].data.at(32);
      int bpp = r + g + b + a;
      auto pixel_hdr = get_data_from_ref(*(u32*)&objs[i].data.at(44));
      auto clut_obj = get_object_from_ref(*(u32*)&objs[i].data.at(60));
      int pixel_size = *(int*)&objs[i].data.at(40);
      int nul = 0;
      if (clut_obj == nullptr) {
        int file_size = pixel_size + 0x36;
        fwrite("BM", 1, 2, bmp);
        fwrite(&file_size, 1, 4, bmp);
        fwrite(&nul, 1, 4, bmp);
        nul = 0x36;
        fwrite(&nul, 1, 4, bmp);
        nul = 0x28;
        fwrite(&nul, 1, 4, bmp);
        fwrite(&w, 1, 4, bmp);
        fwrite(&h, 1, 4, bmp);
        nul = 1;
        fwrite(&nul, 1, 2, bmp);
        fwrite(&bpp, 1, 2, bmp);
        nul = 0;
        fwrite(&nul, 1, 4, bmp);
        fwrite(&nul, 1, 4, bmp);
        nul = 0xB13;
        fwrite(&nul, 1, 4, bmp);
        fwrite(&nul, 1, 4, bmp);
        nul = 0;
        fwrite(&nul, 1, 4, bmp);
        fwrite(&nul, 1, 4, bmp);
        fwrite(pixel_hdr->data.data(), pixel_hdr->data.size(), 1, bmp);
      } else {
        bpp = *(u32*)&objs[i].data.at(64);
        int file_size = *(int*)&clut_obj->data.at(16) + pixel_size + 0x36;
        auto pal_hdr = get_data_from_ref(*(int*)&clut_obj->data.at(12));
        fwrite("BM", 1, 2, bmp);
        fwrite(&file_size, 1, 4, bmp);
        fwrite(&nul, 1, 4, bmp);
        nul = 0x36;
        fwrite(&nul, 1, 4, bmp);
        nul = 0x28;
        fwrite(&nul, 1, 4, bmp);
        fwrite(&w, 1, 4, bmp);
        fwrite(&h, 1, 4, bmp);
        nul = 1;
        fwrite(&nul, 1, 2, bmp);
        fwrite(&bpp, 1, 2, bmp);
        nul = 0;
        fwrite(&nul, 1, 4, bmp);
        fwrite(&nul, 1, 4, bmp);
        nul = 0xB13;
        fwrite(&nul, 1, 4, bmp);
        fwrite(&nul, 1, 4, bmp);
        nul = 0;
        fwrite(&nul, 1, 4, bmp);
        fwrite(&nul, 1, 4, bmp);
        fwrite(pal_hdr->data.data(), pal_hdr->data.size(), 1, bmp);
        fwrite(pixel_hdr->data.data(), pixel_hdr->data.size(), 1, bmp);
      }
      fclose(bmp);
    }
  }
  lg::info("Found and ripped {} images", img_found);
}
