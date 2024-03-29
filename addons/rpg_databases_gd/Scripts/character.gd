extends Container
tool

# Declare member variables here. Examples:
# var a = 2
# var b = "text"
var face_path: String = ""
var chara_path: String = ""
var character_selected: int = 0
var initial_equip_edit: int = -1
var equip_id_array: Array
var equip_edit_array: Array
var initial_equip_id_array: Array
onready var character_button = get_node("VBoxContainer/CharacterList/CharacterList/CharacterButton")
onready var name_text = get_node("VBoxContainer/Body/Data/Top/Top/Name/CenterContainer/NameText")
onready var description_text = get_node("VBoxContainer/Body/Data/Top/Top/Description/CenterContainer/DescText")
onready var face_sprite = get_node("VBoxContainer/Body/Data/Top/Top/Face/FaceSprite")
onready var class_text = get_node("VBoxContainer/Body/Data/Middle/Middle/Class/ClassText")
onready var ilevel_text = get_node("VBoxContainer/Body/Data/Middle/Middle/iLevel/InitLevelText")
onready var mlevel_text = get_node("VBoxContainer/Body/Data/Middle/Middle/mLevel/MaxLevelText")
onready var equip_list = get_node("VBoxContainer/Body/Data/Bottom/Bottom/Equip/EquipContainer/EquipContainer/EquipList")
onready var etype_list = get_node("VBoxContainer/Body/Data/Bottom/Bottom/Iequip/PanelContainer/TypeContainer/TypeList")
onready var iequip_list = get_node("VBoxContainer/Body/Data/Bottom/Bottom/Iequip/PanelContainer/TypeContainer/EquipList")
# Effects
onready var effect_names = get_node("VBoxContainer/Body/Effects/PanelContainer/VBoxContainer/HBoxContainer/EffectNames")
onready var data_type = get_node("VBoxContainer/Body/Effects/PanelContainer/VBoxContainer/HBoxContainer/DataType")
onready var effect_value1 = get_node("VBoxContainer/Body/Effects/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1")
onready var effect_value2 = get_node("VBoxContainer/Body/Effects/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2")


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func start():
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Character")
	character_button.clear()
	for i in range(json_dictionary.size()):
		var chara_data = json_dictionary["chara"+String(i)]
		if i > character_button.get_item_count() - 1:
			character_button.add_item(chara_data["name"])
		else:
			character_button.set_item_text(i, chara_data["name"])
	refresh_data(character_selected)

func refresh_data(id: int):
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Character")
	var chara_data: Dictionary = json_dictionary["chara"+String(id)]
	character_button.set_item_text(id, chara_data["name"])
	name_text.text = chara_data["name"]
	var face: String = chara_data["faceImage"]
	if face != "":
		face_path = face
		face_sprite.texture = load(face)
	if chara_data.has("description"):
		description_text.text = chara_data["description"]
	else:
		description_text.text = ""
	ilevel_text.value = chara_data["initialLevel"]
	mlevel_text.value = chara_data["maxLevel"]
	
	json_dictionary = get_parent().get_parent().call("read_data", "System")
	var w_type_dictionary: Dictionary = json_dictionary["weapons"]
	var a_type_dictionary: Dictionary = json_dictionary["armors"]
	var equip_slots_dictionary: Dictionary = json_dictionary["slots"]
	
	var e_type_dictionary: Dictionary = chara_data["equip_types"]
	equip_list.clear()
	equip_id_array.clear()
	var equip_name: String
	for equip in e_type_dictionary.keys():
		var kind: String = equip[0]
		var type_id: String = String(e_type_dictionary[equip])
		equip_id_array.append(e_type_dictionary[equip])
		match kind:
			"w":
				var w_id = String(equip).erase(0, 1)
				equip_name = "W: " + w_type_dictionary[type_id]
				equip_list.add_item(equip_name)
			"a":
				var a_id = String(equip).erase(0, 1)
				equip_name = "A: " + a_type_dictionary[type_id]
				equip_list.add_item(equip_name)
	var weapon_list: Dictionary = get_parent().get_parent().call("read_data", "Weapon")
	var armor_list: Dictionary = get_parent().get_parent().call("read_data", "Armor")
	etype_list.clear()
	iequip_list.clear()
	var initial_equip_data: Dictionary = chara_data["initial_equip"]
	initial_equip_id_array.clear()
	for equip in equip_slots_dictionary.keys():
		etype_list.add_item(equip_slots_dictionary[equip])
		var kind: String = equip[0]
		var kind_id = String(equip)
		kind_id.erase(0, 1)
		match kind:
			"w":
				var w_id: int = -1
				if int(kind_id) < initial_equip_data.keys().size():
					w_id = int(initial_equip_data[kind_id])
				initial_equip_id_array.append(w_id)
				if w_id >= 0:
					var weapon_data: Dictionary = weapon_list["weapon"+String(w_id)]
					iequip_list.add_item(weapon_data["name"])
				else:
					iequip_list.add_item("None")
			"a":
				var a_id: int = -1
				if int(kind_id) < initial_equip_data.keys().size():
					a_id = int(initial_equip_data[kind_id])
				initial_equip_id_array.append(a_id)
				if a_id >= 0:
					var armor_data: Dictionary = armor_list["armor"+String(a_id)]
					iequip_list.add_item(armor_data["name"])
				else:
					iequip_list.add_item("None")
	json_dictionary = get_parent().get_parent().call("read_data", "Class")
	class_text.clear()
	for i in range(json_dictionary.size()):
		var class_data: Dictionary = json_dictionary["class"+String(i)]
		class_text.add_item(class_data["name"])
	
	if chara_data.has("effects"):
		clear_effect_list()
		var effect_list: Array = chara_data["effects"]
		for effect in effect_list:
			add_effect_list(effect["name"], int(effect["data_id"]), String(effect["value1"]), String(effect["value2"]))

func _on_CharaSaveButton_pressed() -> void:
	save_character_data();
	refresh_data(character_selected)

func _on_AddButton_pressed() -> void:
	character_button.add_item("NewCharacter")
	var id: int = character_button.get_item_count() - 1
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Character")
	var character_data: Dictionary
	var e_type_data: Dictionary
	var e_init_data: Dictionary
	character_data["faceImage"] = ""
	character_data["charaImage"] = ""
	character_data["name"] = "NewCharacter"
	character_data["class"] = 0
	character_data["description"] = ""
	character_data["initialLevel"] = 1
	character_data["maxLevel"] = 99
	e_type_data["w0"] = 0
	e_type_data["w1"] = 1
	e_type_data["a2"] = 0
	e_type_data["a3"] = 3
	e_init_data["0"] = -1
	e_init_data["1"] = -1
	e_init_data["2"] = -1
	e_init_data["3"] = -1
	character_data["initial_equip"] = e_init_data
	character_data["equip_types"] = e_type_data
	json_dictionary["chara"+String(id)] = character_data
	get_parent().get_parent().call("store_data", "Character", json_dictionary)

func _on_RemoveCharacterButton_pressed():
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Character")
	if json_dictionary.keys().size() > 1:
		var chara: int = character_selected
		while chara < (json_dictionary.keys().size() - 1):
			json_dictionary["chara"+String(chara)] = json_dictionary["chara"+String(chara+1)]
			chara += 1
		json_dictionary.erase("chara"+String(chara))
		get_parent().get_parent().call("store_data", "Character", json_dictionary)
		character_button.remove_item(character_selected)
		if character_selected == 0:
			character_button.select(character_selected + 1)
			character_selected += 1
		else:
			character_button.select(character_selected + 1)
			character_selected -= 1
		character_button.select(character_selected)
		refresh_data(character_selected)

func _on_Search_pressed() -> void:
	$FaceSearch.popup_centered()

func _on_FaceSearch_file_selected(file: String) -> void:
	face_path = file
	set_face_image(file)

func set_face_image(path: String) -> void:
	$FaceSprite.texture = load(path)

func save_character_data() -> void:
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Character")
	var chara_data: Dictionary = json_dictionary["chara"+String(character_selected)]
	var equip_type_data: Dictionary = chara_data["equip_types"]
	var initial_equip_data: Dictionary = chara_data["initial_equip"]
	var effect_list: Array
	chara_data["faceImage"] = face_path
	chara_data["charaImage"] = ""
	chara_data["name"] = name_text.text
	character_button.set_item_text(character_selected, name_text.text)
	chara_data["class"] = class_text.selected
	chara_data["description"] = description_text.text
	chara_data["initialLevel"] = ilevel_text.value
	chara_data["maxLevel"] = mlevel_text.value
	equip_type_data.clear()
	var equip_items: int = equip_list.get_item_count()
	for i in range(equip_items):
		var kind: String = equip_list.get_item_text(i)[0]
		match kind:
			"W":
				equip_type_data["w"+String(i)] = equip_id_array[i]
			"A":
				equip_type_data["a"+String(i)] = equip_id_array[i]
	chara_data["equip_types"] = equip_type_data
	var slot_items: int = iequip_list.get_item_count()
	for i in range(slot_items):
		var data = initial_equip_id_array[i]
		initial_equip_data[String(i)] = data
	chara_data["initial_equip"] = initial_equip_data
	var effect_size: int = effect_names.get_item_count()
	for i in range(effect_size):
		var effect_data: Dictionary
		effect_data["name"] = effect_names.get_item_text(i)
		effect_data["data_id"] = data_type.get_item_text(i)
		effect_data["value1"] = effect_value1.get_item_text(i)
		effect_data["value2"] = effect_value2.get_item_text(i)
		effect_list.append(effect_data)
	chara_data["effects"] = effect_list
	get_parent().get_parent().call("store_data", "Character", json_dictionary)

func _on_CharacterButton_item_selected(id: int) -> void:
	character_selected = id
	refresh_data(id)

func _on_CancelButton_pressed() -> void:
	$AddEquip.hide()

func _on_AddEquipTypeButton_pressed() -> void:
	$AddEquip.popup_centered()
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "System")
	var w_type_data: Dictionary = json_dictionary["weapons"]
	for i in w_type_data.size():
		if i > $AddEquip/EquipLabel/EquipButton.get_item_count() - 1:
			$AddEquip/EquipLabel/EquipButton.add_item(w_type_data[String(i)])
		else:
			$AddEquip/EquipLabel/EquipButton.set_item_text(i, w_type_data[String(i)])

func _on_RemoveEquipTypeButton_pressed() -> void:
	var selected: int = equip_list.get_selected_items()[0]
	equip_id_array.remove(selected)
	equip_list.remove_item(selected)

func _on_OkButton_pressed() -> void:
	var kind: int = $AddEquip/TypeLabel/TypeButton.selected
	var item: int = $AddEquip/EquipLabel/EquipButton.selected
	equip_id_array.append(int(item))
	var item_text: String = $AddEquip/EquipLabel/EquipButton.text
	match kind:
		0:
			equip_list.add_item("W: "+item_text)
		1:
			equip_list.add_item("A: "+item_text)
	$AddEquip.hide()

func _on_TypeButton_item_selected(index: int) -> void:
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "System")
	$AddEquip/EquipLabel/EquipButton.clear()
	match index:
		0:
			var w_type_data: Dictionary = json_dictionary["weapons"]
			for i in range(w_type_data.size()):
				if i > $AddEquip/EquipLabel/EquipButton.get_item_count() - 1:
					$AddEquip/EquipLabel/EquipButton.add_item(w_type_data[String(i)])
				else:
					$AddEquip/EquipLabel/EquipButton.set_item_text(i, w_type_data[String(i)])
		1:
			var a_type_data: Dictionary = json_dictionary["armors"]
			for i in range(a_type_data.size()):
				if i > $AddEquip/EquipLabel/EquipButton.get_item_count() - 1:
					$AddEquip/EquipLabel/EquipButton.add_item(a_type_data[String(i)])
				else:
					$AddEquip/EquipLabel/EquipButton.set_item_text(i, a_type_data[String(i)])

func _on_EquipList_item_activated(index: int) -> void:
	initial_equip_edit = index
	equip_edit_array.append("-1")
	$InitialEquipChange/Label.text = etype_list.get_item_text(index)
	$InitialEquipChange/Label/OptionButton.clear()
	$InitialEquipChange/Label/OptionButton.add_item("None")
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Character")
	var chara_data: Dictionary = json_dictionary["chara"+String(character_selected)]
	json_dictionary = get_parent().get_parent().call("read_data", "System")
	var slots_data: Dictionary = json_dictionary["slots"]
	var equip_types_data: Dictionary = chara_data["equip_types"]
	if slots_data.has("w"+String(index)):
		var weapon_list: Dictionary = get_parent().get_parent().call("read_data", "Weapon")
		var weapon_array: Array
		for key in equip_types_data.keys():
			var kind: String = key[0]
			if kind == "w":
				weapon_array.append(int(equip_types_data[key]))
		for weapon in weapon_list.keys():
			var weapon_data: Dictionary = weapon_list[weapon]
			if weapon_data["weapon_type"] in weapon_array:
				var weapon_name = String(weapon)
				weapon_name.erase(0, 6)
				equip_edit_array.append(weapon_name)
				$InitialEquipChange/Label/OptionButton.add_item(weapon_data["name"])
	elif slots_data.has("a"+String(index)):
		var armor_list: Dictionary = get_parent().get_parent().call("read_data", "Armor")
		var armor_array: Array
		for key in equip_types_data.keys():
			var kind: String = key[0]
			if kind == "a":
				armor_array.append(int(equip_types_data[key]))
		for armor in armor_list.keys():
			var armor_data: Dictionary = armor_list[armor]
			if armor_data["armor_type"] in armor_array:
				var armor_name = String(armor)
				armor_name.erase(0, 5)
				equip_edit_array.append(armor_name)
				$InitialEquipChange/Label/OptionButton.add_item(armor_data["name"])
	$InitialEquipChange.popup_centered()

func _on_CancelInitialEquipButton_pressed() -> void:
	initial_equip_edit = -1
	equip_edit_array.clear()
	$InitialEquipChange.hide()

func _on_OkInitialEquipButton_pressed() -> void:
	var text: String = $InitialEquipChange/Label/OptionButton.text
	var selected: int = $InitialEquipChange/Label/OptionButton.selected
	iequip_list.set_item_text(initial_equip_edit, text)
	initial_equip_id_array[initial_equip_edit] = equip_edit_array[selected]
	initial_equip_edit = -1
	equip_edit_array.clear()
	$InitialEquipChange.hide()

func _on_CharacterAddEffectButton_pressed() -> void:
	get_parent().get_parent().call("open_effect_manager", "Character")

func _on_CharacterRemoveEffectButton_pressed() -> void:
	var id: int = effect_names.get_selected_items()[0]
	effect_names.remove_item(id)
	data_type.remove_item(id)
	effect_value1.remove_item(id)
	effect_value2.remove_item(id)

func add_effect_list(name: String, data_id: int, value1: String, value2: String) -> void:
	effect_names.add_item(name)
	data_type.add_item(String(data_id))
	effect_value1.add_item(value1)
	effect_value2.add_item(value2)

func clear_effect_list() -> void:
	effect_names.clear()
	data_type.clear()
	effect_value1.clear()
	effect_value2.clear()

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
