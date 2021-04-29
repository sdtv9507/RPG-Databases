extends Container
tool

# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var edited_field: int = -1
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func start() -> void:
	var json_dictionary = get_parent().get_parent().call("read_data", "System")
	var stats_data = json_dictionary["stats"]
	var weapons_data = json_dictionary["weapons"]
	var armors_data = json_dictionary["armors"]
	var elements_data = json_dictionary["elements"]
	var slots_data = json_dictionary["slots"]
	var skills_data
	if json_dictionary.has("skills"):
		skills_data = json_dictionary["skills"]
	else:
		skills_data["0"] = "Skills"
		skills_data["1"] = "Magic"
		json_dictionary["skills"] = skills_data
		get_parent().get_parent().call("store_data", "System", json_dictionary)
	$StatsLabel/StatsContainer/StatsBoxContainer/StatsList.clear()
	$WeaponTypesLabel/WeaponTypesContainer/WpBoxContainer/WeaponList.clear()
	$ArmorTypesLabel/ArmorTypesContainer/ArBoxContainer/ArmorList.clear()
	$ElementLabel/ElementContainer/EleBoxContainer/ElementList.clear()
	$EquipmentLabel/EquipContainer/SetContainer/SetDivisor/KindList.clear()
	$EquipmentLabel/EquipContainer/SetContainer/SetDivisor/TypeList.clear()
	$SkillTypesLabel/SkillTypeContainer/VBoxContainer/SkillTypeList.clear()
	for i in range(stats_data.size()):
		var item = $StatsLabel/StatsContainer/StatsBoxContainer/StatsList
		item.add_item(stats_data[String(i)])
	for i in range(weapons_data.size()):
		var item = $WeaponTypesLabel/WeaponTypesContainer/WpBoxContainer/WeaponList
		item.add_item(weapons_data[String(i)])
	for i in range(armors_data.size()):
		var item = $ArmorTypesLabel/ArmorTypesContainer/ArBoxContainer/ArmorList
		item.add_item(armors_data[String(i)])
	for i in range(elements_data.size()):
		var item = $ElementLabel/ElementContainer/EleBoxContainer/ElementList
		item.add_item(elements_data[String(i)])
	for i in range(skills_data.size()):
		var item = $SkillTypesLabel/SkillTypeContainer/VBoxContainer/SkillTypeList
		item.add_item(skills_data[String(i)])
	for id in slots_data.keys():
		var kind = $EquipmentLabel/EquipContainer/SetContainer/SetDivisor/KindList
		var kindId: String = String(id[0])
		print(kindId)
		var type = $EquipmentLabel/EquipContainer/SetContainer/SetDivisor/TypeList
		match (kindId):
			"w":
				kind.add_item("Weapon")
				type.add_item(slots_data[id])
			"a":
				kind.add_item("Armor")
				type.add_item(slots_data[id])

func save_data():
	save_stats()
	save_weapons()
	save_armors()
	save_elements()
	save_slots()
	save_skills()

func save_stats():
	var json_dictionary = get_parent().get_parent().call("read_data", "System")
	var stats_data: Dictionary
	var stat_size = $StatsLabel/StatsContainer/StatsBoxContainer/StatsList.get_item_count()
	for i in range(stat_size):
		var text = $StatsLabel/StatsContainer/StatsBoxContainer/StatsList.get_item_text(i)
		stats_data[String(i)] = text
	json_dictionary["stats"] = stats_data
	get_parent().get_parent().call("store_data", "System", json_dictionary)

func save_weapons():
	var json_dictionary = get_parent().get_parent().call("read_data", "System")
	var weapons_data: Dictionary
	var weapon_size = $WeaponTypesLabel/WeaponTypesContainer/WpBoxContainer/WeaponList.get_item_count()
	for i in range(weapon_size):
		var text = $WeaponTypesLabel/WeaponTypesContainer/WpBoxContainer/WeaponList.get_item_text(i)
		weapons_data[String(i)] = text
	json_dictionary["weapons"] = weapons_data
	get_parent().get_parent().call("store_data", "System", json_dictionary)

func save_armors():
	var json_dictionary = get_parent().get_parent().call("read_data", "System")
	var armors_data: Dictionary
	var armor_size = $ArmorTypesLabel/ArmorTypesContainer/ArBoxContainer/ArmorList.get_item_count()
	for i in range(armor_size):
		var text = $ArmorTypesLabel/ArmorTypesContainer/ArBoxContainer/ArmorList.get_item_text(i)
		armors_data[String(i)] = text
	json_dictionary["armors"] = armors_data
	get_parent().get_parent().call("store_data", "System", json_dictionary)

func save_elements():
	var json_dictionary = get_parent().get_parent().call("read_data", "System")
	var elements_data: Dictionary
	var element_size = $ElementLabel/ElementContainer/EleBoxContainer/ElementList.get_item_count()
	for i in range(element_size):
		var text = $ElementLabel/ElementContainer/EleBoxContainer/ElementList.get_item_text(i)
		elements_data[String(i)] = text
	json_dictionary["elements"] = elements_data
	get_parent().get_parent().call("store_data", "System", json_dictionary)

func save_skills():
	var json_dictionary = get_parent().get_parent().call("read_data", "System")
	var skills_data: Dictionary
	var skill_size = $SkillTypesLabel/SkillTypeContainer/VBoxContainer/SkillTypeList.get_item_count()
	for i in range(skill_size):
		var text = $SkillTypesLabel/SkillTypeContainer/VBoxContainer/SkillTypeList.get_item_text(i)
		skills_data[String(i)] = text
	json_dictionary["skills"] = skills_data
	get_parent().get_parent().call("store_data", "System", json_dictionary)

func save_slots():
	var json_dictionary = get_parent().get_parent().call("read_data", "System")
	var slots_data: Dictionary
	var slot_size = $EquipmentLabel/EquipContainer/SetContainer/SetDivisor/KindList.get_item_count()
	for i in range(slot_size):
		var kind = $EquipmentLabel/EquipContainer/SetContainer/SetDivisor/KindList.get_item_text(i)
		var id = ""
		match (kind):
			"Weapon":
				id = "w"
			"Armor":
				id = "a"
		var text = $EquipmentLabel/EquipContainer/SetContainer/SetDivisor/TypeList.get_item_text(i)
		id += String(i)
		slots_data[String(id)] = text
	json_dictionary["slots"] = slots_data
	get_parent().get_parent().call("store_data", "System", json_dictionary)

func _on_OKButton_pressed():
	var name = $EditField/FieldName.text
	if (name != ""):
		if (edited_field == 0):
			$StatsLabel/StatsContainer/StatsBoxContainer/StatsList.add_item(name)
		elif (edited_field == 1):
			$WeaponTypesLabel/WeaponTypesContainer/WpBoxContainer/WeaponList.add_item(name)
		elif (edited_field == 2):
			$ArmorTypesLabel/ArmorTypesContainer/ArBoxContainer/ArmorList.add_item(name)
		elif (edited_field == 3):
			$ElementLabel/ElementContainer/EleBoxContainer/ElementList.add_item(name)
		elif (edited_field == 4):
			$SkillTypesLabel/SkillTypeContainer/VBoxContainer/SkillTypeList.add_item(name)
		save_data()
	$EditField.hide()
	edited_field = -1

func _on_CancelButton_pressed():
	edited_field = -1
	$EditField.hide()

func _on_EditField_popup_hide():
	edited_field = -1
	$EditField.hide()

func _on_AddStat_pressed():
	edited_field = 0
	$EditField.window_title = "Add Stat"
	$EditField.popup_centered(Vector2(392, 95))

func _on_RemoveStat_pressed():
	var index = $StatsLabel/StatsContainer/StatsBoxContainer/StatsList.get_selected_items()[0]
	if (index > -1):
		$StatsLabel/StatsContainer/StatsBoxContainer/StatsList.remove_item(index)
		save_data()

func _on_AddWeapon_pressed():
	edited_field = 1
	$EditField.window_title = "Add Weapon"
	$EditField.popup_centered(Vector2(392, 95))

func _on_RemoveWeapon_pressed():
	var index = $WeaponTypesLabel/WeaponTypesContainer/WpBoxContainer/WeaponList.get_selected_items()[0]
	if (index > -1):
		$WeaponTypesLabel/WeaponTypesContainer/WpBoxContainer/WeaponList.remove_item(index)
		save_data()

func _on_AddArmor_pressed():
	edited_field = 2
	$EditField.window_title = "Add Armor"
	$EditField.popup_centered(Vector2(392, 95))

func _on_RemoveArmor_pressed():
	var index = $ArmorTypesLabel/ArmorTypesContainer/ArBoxContainer/ArmorList.get_selected_items()[0]
	if (index > -1):
		$ArmorTypesLabel/ArmorTypesContainer/ArBoxContainer/ArmorList.remove_item(index)
		save_data()

func _on_AddElement_pressed():
	edited_field = 3
	$EditField.window_title = "Add Element"
	$EditField.popup_centered(Vector2(392, 95))

func _on_RemoveElement_pressed():
	var index = $ElementLabel/ElementContainer/EleBoxContainer/ElementList.get_selected_items()[0]
	if (index > -1):
		$ElementLabel/ElementContainer/EleBoxContainer/ElementList.remove_item(index)
		save_data()

func _on_AddSkillType_pressed():
	edited_field = 4
	$EditField.window_title = "Add Skill Type"
	$EditField.popup_centered(Vector2(392, 95))

func _on_RemoveSKillType_pressed():
	var index = $SkillTypesLabel/SkillTypeContainer/VBoxContainer/SkillTypeList.get_selected_items()[0]
	if (index > -1):
		$SkillTypesLabel/SkillTypeContainer/VBoxContainer/SkillTypeList.remove_item(index)
		save_data()

func _on_AddSet_pressed():
	$AddSlot.popup_centered()

func _on_RemoveSet_pressed():
	var index = $EquipmentLabel/EquipContainer/SetContainer/SetDivisor/TypeList.get_selected_items()[0]
	if (index > -1):
		$EquipmentLabel/EquipContainer/SetContainer/SetDivisor/KindList.remove_item(index)
		$EquipmentLabel/EquipContainer/SetContainer/SetDivisor/TypeList.remove_item(index)
		save_data()

func _on_AddSlotCancelButton_pressed():
	$AddSlot.hide()

func _on_AddSlotOkButton_pressed():
	var kind = $AddSlot/TypeLabel/TypeButton.selected
	match (kind):
		0:
			$EquipmentLabel/EquipContainer/SetContainer/SetDivisor/KindList.add_item("Weapon")
		1:
			$EquipmentLabel/EquipContainer/SetContainer/SetDivisor/KindList.add_item("Armor")
	var name = $AddSlot/NameLabel/NameEdit.text
	$EquipmentLabel/EquipContainer/SetContainer/SetDivisor/TypeList.add_item(name)
	save_data()
	$AddSlot.hide()
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
