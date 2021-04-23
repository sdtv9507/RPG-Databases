extends WindowDialog
tool

var effect_manager_tab: String = ""
var effect_data_type: String = ""
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	var database_file: File = File.new()
	if (!database_file.file_exists("res://databases/System.json")):
		database_file.open("res://databases/System.json", File.WRITE)
		var system_list: Dictionary
		var stats_data: Dictionary
		var weapon_type_data: Dictionary
		var armor_type_data: Dictionary
		var element_data: Dictionary
		var slots_data: Dictionary
		var skill_type_data: Dictionary
		stats_data["0"] = "hp"
		stats_data["1"] = "mp"
		stats_data["2"] = "atk"
		stats_data["3"] = "def"
		stats_data["4"] = "int"
		stats_data["5"] = "res"
		stats_data["6"] = "spd"
		stats_data["7"] = "luk"
		weapon_type_data["0"] = "Sword"
		weapon_type_data["1"] = "Spear"
		weapon_type_data["2"] = "Axe"
		weapon_type_data["3"] = "Staff"
		armor_type_data["0"] = "Armor"
		armor_type_data["1"] = "Robe"
		armor_type_data["2"] = "Shield"
		armor_type_data["3"] = "Hat"
		armor_type_data["4"] = "Accessory"
		element_data["0"] = "Physical"
		element_data["1"] = "Fire"
		element_data["2"] = "Ice"
		element_data["3"] = "Wind"
		slots_data["w0"] = "Weapon"
		slots_data["a1"] = "Head"
		slots_data["a2"] = "Body"
		slots_data["a3"] = "Accessory"
		skill_type_data["0"] = "Skills"
		skill_type_data["1"] = "Magic"
		system_list["stats"] = stats_data
		system_list["weapons"] = weapon_type_data
		system_list["armors"] = armor_type_data
		system_list["elemets"] = element_data
		system_list["slots"] = slots_data
		system_list["skills"] = skill_type_data
		database_file.store_string(JSON.print(system_list))
		database_file.close()
	if (!database_file.file_exists("res://databases/Character.json")):
		database_file.open("res://databases/Character.json", File.WRITE)
		var character_list: Dictionary
		var character_data: Dictionary
		var equip_type_data: Dictionary
		var initial_equip_data: Dictionary
		character_data["faceImage"] = ""
		character_data["charaImage"] = ""
		character_data["name"] = "Kate"
		character_data["class"] = 0
		character_data["description"] = ""
		character_data["initialLevel"] = 1
		character_data["maxLevel"] = 99
		equip_type_data["w0"] = 0
		equip_type_data["w1"] = 1
		equip_type_data["a2"] = 0
		equip_type_data["a3"] = 3
		initial_equip_data["0"] = -1
		initial_equip_data["1"] = -1
		initial_equip_data["2"] = -1
		initial_equip_data["3"] = -1
		character_data["initial_equip"] = initial_equip_data
		character_data["equip_types"] = equip_type_data
		character_list["chara0"] = character_data
		database_file.store_line(JSON.print(character_list))
		database_file.close()
	if (!database_file.file_exists("res://databases/Skill.json")):
		database_file.open("res://databases/Skill.json", File.WRITE)
		var skill_data: Dictionary
		var skill_list: Dictionary
		skill_data["name"] = "Double Attack"
		skill_data["icon"] = ""
		skill_data["description"] = "Attacks an enemy twice"
		skill_data["skill_type"] = 0
		skill_data["mp_cost"] = 4
		skill_data["tp_cost"] = 2
		skill_data["target"] = 1
		skill_data["usable"] = 1
		skill_data["success"] = 95
		skill_data["hit_type"] = 1
		skill_data["damage_type"] = 1
		skill_data["element"] = 0
		skill_data["formula"] = "atk * 4 - def * 2"
		skill_list["skill0"] = skill_data
		database_file.store_line(JSON.print(skill_list))
		database_file.close()
	if (!database_file.file_exists("res://databases/Class.json")):
		database_file.open("res://databases/Class.json", File.WRITE)
		var class_data: Dictionary
		var class_list: Dictionary
		var class_stats: Dictionary
		var skill_list: Dictionary
		class_data["name"] = "Warrior"
		class_data["icon"] = ""
		class_data["experience"] = "level * 30"
		class_stats["hp"] = "level * 25 + 10"
		class_stats["mp"] = "level * 15 + 5"
		class_stats["atk"] = "level * 5 + 3"
		class_stats["def"] = "level * 5 + 3"
		class_stats["int"] = "level * 5 + 3"
		class_stats["res"] = "level * 5 + 3"
		class_stats["spd"] = "level * 5 + 3"
		class_stats["luk"] = "level * 5 + 3"
		skill_list[0] = 5
		class_data["skill_list"] = skill_list
		class_data["stat_list"] = class_stats
		class_list["class0"] = class_data
		database_file.store_line(JSON.print(class_list))
		database_file.close()
	if (!database_file.file_exists("res://databases/Item.json")):
		database_file.open("res://databases/Item.json", File.WRITE)
		var item_list: Dictionary
		var item_data: Dictionary
		item_data["name"] = "Potion"
		item_data["icon"] = ""
		item_data["description"] = "Heals 50HP to one ally"
		item_data["item_type"] = 0
		item_data["price"] = 50
		item_data["consumable"] = 0
		item_data["target"] = 7
		item_data["usable"] = 0
		item_data["success"] = 100
		item_data["hit_type"] = 0
		item_data["damage_type"] = 3
		item_data["element"] = 0
		item_data["formula"] = "50"
		item_list["item0"] = item_data
		database_file.store_line(JSON.print(item_list))
		database_file.close()
	if (!database_file.file_exists("res://databases/Weapon.json")):
		database_file.open("res://databases/Weapon.json", File.WRITE)
		var weapon_list: Dictionary
		var weapon_data: Dictionary
		var weapon_stats: Dictionary
		weapon_data["name"] = "Broad Sword"
		weapon_data["icon"] = ""
		weapon_data["description"] = "A light and easy to use sword"
		weapon_data["weapon_type"] = 0
		weapon_data["slot_type"] = 0
		weapon_data["price"] = 50
		weapon_data["element"] = 0
		weapon_stats["hp"] = "0"
		weapon_stats["mp"] = "0"
		weapon_stats["atk"] = "10"
		weapon_stats["def"] = "2"
		weapon_stats["int"] = "2"
		weapon_stats["res"] = "1"
		weapon_stats["spd"] = "0"
		weapon_stats["luk"] = "0"
		weapon_data["stat_list"] = weapon_stats
		weapon_list["weapon0"] = weapon_data
		database_file.store_line(JSON.print(weapon_list))
		database_file.close()
	if (!database_file.file_exists("res://databases/Armor.json")):
		database_file.open("res://databases/Armor.json", File.WRITE)
		var armor_list: Dictionary
		var armor_data: Dictionary
		var armor_stats: Dictionary
		armor_data["name"] = "Clothes"
		armor_data["icon"] = ""
		armor_data["description"] = "Light Clothes"
		armor_data["armor_type"] = 0
		armor_data["slot_type"] = 0
		armor_data["price"] = 50
		armor_stats["hp"] = "0"
		armor_stats["mp"] = "0"
		armor_stats["atk"] = "10"
		armor_stats["def"] = "2"
		armor_stats["int"] = "2"
		armor_stats["res"] = "1"
		armor_stats["spd"] = "0"
		armor_stats["luk"] = "0"
		armor_data["stat_list"] = armor_stats
		armor_list["armor0"] = armor_data
		database_file.store_line(JSON.print(armor_list))
		database_file.close()
	if (!database_file.file_exists("res://databases/Enemy.json")):
		database_file.open("res://databases/Enemy.json", File.WRITE)
		var enemy_list: Dictionary
		var enemy_data: Dictionary
		var stats_data: Dictionary
		var drop_data: Dictionary
		enemy_data["name"] = "Slime"
		enemy_data["graphicImage"] = ""
		stats_data["hp"] = "150"
		stats_data["mp"] = "50"
		stats_data["atk"] = "18"
		stats_data["def"] = "16"
		stats_data["int"] = "8"
		stats_data["res"] = "4"
		stats_data["spd"] = "12"
		stats_data["luk"] = "10"
		drop_data["i0"] = 80
		enemy_data["experience"] = 6
		enemy_data["money"] = 50
		enemy_data["stat_list"] = stats_data
		enemy_data["drop_list"] = drop_data
		enemy_list["enemy0"] = enemy_data
		database_file.store_line(JSON.print(enemy_list))
		database_file.close()
	if (!database_file.file_exists("res://databases/State.json")):
		database_file.open("res://databases/State.json", File.WRITE)
		var state_list: Dictionary
		var state_data: Dictionary
		var erase_condition: Dictionary
		var messages: Dictionary
		var custom_erase_conditions: Dictionary
		state_data["name"] = "Death"
		state_data["icon"] = ""
		state_data["restriction"] = 4
		state_data["priority"] = 100
		erase_condition["turns_min"] = 0
		erase_condition["turns_max"] = 0
		erase_condition["erase_damage"] = 0
		erase_condition["erase_setps"] = 0
		state_data["erase_conditions"] = erase_condition
		messages["0"] = "Insert a custom message"
		state_data["messages"] = messages
		custom_erase_conditions["0"] = "Insert a custom formula for erase state"
		state_data["custom_erase_conditions"] = custom_erase_conditions

		state_list["state0"] = state_data
		database_file.store_line(JSON.print(state_list))
		database_file.close()
	if (!database_file.file_exists("res://databases/Effect.json")):
		database_file.open("res://databases/Effect.json", File.WRITE)
		var effect_list: Dictionary
		var effect_data: Dictionary
		var show_list: Dictionary
		var value2: Dictionary
		effect_data["name"] = "hp_recovery"
		show_list["show"] = false
		show_list["data"] = ""
		effect_data["data_type"] = show_list
		effect_data["value1"] = 1
		value2["show"] = true
		value2["data"] = 2
		effect_data["value2"] = value2

		effect_list["effect0"] = effect_data
		database_file.store_line(JSON.print(effect_list))
		database_file.close()
	
	database_file.close()
	$Tabs/Character.call("start")

func read_data(file: String) -> Dictionary:
	var database_file: File = File.new()
	database_file.open("res://databases/" + file + ".json", File.READ)
	var json_as_text: String = database_file.get_as_text()
	database_file.close()
	var json_parsed: JSONParseResult = JSON.parse(json_as_text)
	return json_parsed.result

func store_data(file: String, data: Dictionary) -> void:
	var database_file: File = File.new()
	database_file.open("res://databases/" + file + ".json", File.WRITE)
	database_file.store_string(JSON.print(data))
	database_file.close()

func open_effect_manager(tab: String) -> void:
	var effect_list: Dictionary = read_data("Effect")
	var effect_data: Dictionary
	$EffectManager/HBoxContainer/EffectList.clear()
	for i in range(effect_list.size()):
		effect_data = effect_list["effect"+String(i)]
		$EffectManager/HBoxContainer/EffectList.add_item(effect_data["name"])
	$EffectManager/HBoxContainer/EffectList.select(0)
	_on_EffectList_item_selected(0)
	effect_manager_tab = tab
	$EffectManager.popup_centered()

func _on_EffectList_item_selected(index: int) -> void:
	var effect_list: Dictionary = read_data("Effect")
	var json_dictionary: Dictionary
	var json_data: Dictionary
	var effect_data: Dictionary = effect_list["effect"+String(index)]
	var data_types: Dictionary = effect_data["data_type"]
	var value2: Dictionary = effect_data["value2"]
	$EffectManager/HBoxContainer/VBoxContainer/DataList.clear()
	if data_types["show"] == false:
		$EffectManager/HBoxContainer/VBoxContainer/DataType.hide()
		$EffectManager/HBoxContainer/VBoxContainer/DataList.hide()
		effect_data_type = "Disabled"
	else:
		$EffectManager/HBoxContainer/VBoxContainer/DataType.show()
		$EffectManager/HBoxContainer/VBoxContainer/DataList.show()
		var type: String = data_types["data"]
		effect_data_type = data_types["data"]
		match type:
			"States":
				json_dictionary = read_data("State")
				for i in range(json_dictionary.size()):
					json_data = json_dictionary["state"+String(i)]
					$EffectManager/HBoxContainer/VBoxContainer/DataList.add_item(json_data["name"])
			"Stats":
				json_dictionary = read_data("System")
				json_data = json_dictionary["stats"]
				for i in range(json_dictionary.size()):
					$EffectManager/HBoxContainer/VBoxContainer/DataList.add_item(String(i))
			"Weapon Types":
				json_dictionary = read_data("System")
				json_data = json_dictionary["weapons"]
				for i in range(json_dictionary.size()):
					$EffectManager/HBoxContainer/VBoxContainer/DataList.add_item(String(i))
			"Armor Types":
				json_dictionary = read_data("System")
				json_data = json_dictionary["armors"]
				for i in range(json_dictionary.size()):
					$EffectManager/HBoxContainer/VBoxContainer/DataList.add_item(String(i))
			"Elements":
				json_dictionary = read_data("System")
				json_data = json_dictionary["elements"]
				for i in range(json_dictionary.size()):
					$EffectManager/HBoxContainer/VBoxContainer/DataList.add_item(String(i))
			"Skill Types":
				json_dictionary = read_data("System")
				json_data = json_dictionary["skills"]
				for i in range(json_dictionary.size()):
					$EffectManager/HBoxContainer/VBoxContainer/DataList.add_item(String(i))
	match effect_data["value1"]:
		0:
			$EffectManager/HBoxContainer/VBoxContainer/Value1/LineEdit.show()
			$EffectManager/HBoxContainer/VBoxContainer/Value1/SpinBox.hide()
		1:
			$EffectManager/HBoxContainer/VBoxContainer/Value1/LineEdit.hide()
			$EffectManager/HBoxContainer/VBoxContainer/Value1/SpinBox.show()
			$EffectManager/HBoxContainer/VBoxContainer/Value1/SpinBox.rounded = true
		2:
			$EffectManager/HBoxContainer/VBoxContainer/Value1/LineEdit.hide()
			$EffectManager/HBoxContainer/VBoxContainer/Value1/SpinBox.show()
			$EffectManager/HBoxContainer/VBoxContainer/Value1/SpinBox.rounded = false
	if value2["show"] == false:
		$EffectManager/HBoxContainer/VBoxContainer/Value2.hide()
	else:
		$EffectManager/HBoxContainer/VBoxContainer/Value2.show()
		match value2["data"]:
			0:
				$EffectManager/HBoxContainer/VBoxContainer/Value2/LineEdit.show()
				$EffectManager/HBoxContainer/VBoxContainer/Value2/SpinBox.hide()
			1:
				$EffectManager/HBoxContainer/VBoxContainer/Value2/LineEdit.hide()
				$EffectManager/HBoxContainer/VBoxContainer/Value2/SpinBox.show()
				$EffectManager/HBoxContainer/VBoxContainer/Value2/SpinBox.rounded = true
			2:
				$EffectManager/HBoxContainer/VBoxContainer/Value2/LineEdit.hide()
				$EffectManager/HBoxContainer/VBoxContainer/Value2/SpinBox.show()
				$EffectManager/HBoxContainer/VBoxContainer/Value2/SpinBox.rounded = false

func _on_AddEffectConfirm_pressed() -> void:
	var id: int = $EffectManager/HBoxContainer/EffectList.get_selected_items()[0]
	var name: String = $EffectManager/HBoxContainer/EffectList.get_item_text(id)
	var data_type: int = -1
	if (effect_data_type != "Disabled"):
		data_type = $EffectManager/HBoxContainer/VBoxContainer/DataList.selected
	var value1: String
	if $EffectManager/HBoxContainer/VBoxContainer/Value1/LineEdit.visible:
		value1 = $EffectManager/HBoxContainer/VBoxContainer/Value1/LineEdit.text
	elif $EffectManager/HBoxContainer/VBoxContainer/Value1/SpinBox.rounded == true:
		value1 = String($EffectManager/HBoxContainer/VBoxContainer/Value1/SpinBox.value)
	else:
		value1 = String($EffectManager/HBoxContainer/VBoxContainer/Value1/SpinBox.value)
	var value2: String = ""
	if $EffectManager/HBoxContainer/VBoxContainer/Value2.visible:
		if $EffectManager/HBoxContainer/VBoxContainer/Value2/LineEdit.visible:
			value2 = $EffectManager/HBoxContainer/VBoxContainer/Value2/LineEdit.text
		elif $EffectManager/HBoxContainer/VBoxContainer/Value2/SpinBox.rounded == true:
			value2 = String($EffectManager/HBoxContainer/VBoxContainer/Value2/SpinBox.value)
		else:
			value2 = String($EffectManager/HBoxContainer/VBoxContainer/Value2/SpinBox.value)
	else:
		value2 = "0"
	get_node("Tabs"+effect_manager_tab).call("add_effect_list", name, data_type, value1, value2)
	effect_manager_tab = ""
	effect_data_type = ""
	$EffectManager.hide()

func _on_Tabs_tab_changed(tab: int) -> void:
	if tab == 0:
		$Tabs/Character.call("start")
	if tab == 1:
		$Tabs/Class.call("start")
	if tab == 2:
		$Tabs/Skill.call("start")
	if tab == 3:
		$Tabs/Item.call("start")
	if tab == 4:
		$Tabs/Weapon.call("start")
	if tab == 5:
		$Tabs/Armor.call("start")
	if tab == 6:
		$Tabs/Enemy.call("start")
	if tab == 7:
		$Tabs/States.call("start")
	if tab == 8:
		$Tabs/Effects.call("start")
	if tab == 9:
		$Tabs/System.call("start")

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
