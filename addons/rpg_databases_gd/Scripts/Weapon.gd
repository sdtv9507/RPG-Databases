extends Container
tool

# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var icon_path: String = ""
var weapon_selected: int = 0
var stat_edit: int = -1
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func start() -> void:
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Weapon")
	$WeaponButton.clear()
	for i in range(json_dictionary.size()):
		var weapon_data: Dictionary = json_dictionary["weapon"+String(i)]
		if i > $WeaponButton.get_item_count() - 1:
			$WeaponButton.add_item(weapon_data["name"])
		else:
			$WeaponButton.set_item_text(i, weapon_data["name"])
	json_dictionary = get_parent().get_parent().call("read_data", "System")
	var system_data: Dictionary = json_dictionary["elements"]
	for i in range(system_data.size()):
		if i > $ElementLabel/ElementButton.get_item_count() - 1:
			$ElementLabel/ElementButton.add_item(system_data[String(i)])
		else:
			$ElementLabel/ElementButton.set_item_text(i, system_data[String(i)])
	system_data = json_dictionary["weapon"]
	for i in range(system_data.size()):
		if i > $WTypeLabel/WTypeButton.get_item_count() - 1:
			$WTypeLabel/WTypeButton.add_item(system_data[String(i)])
		else:
			$WTypeLabel/WTypeButton.set_item_text(i, system_data[String(i)])
	system_data = json_dictionary["slots"]
	var final_id: int = 0
	for string in system_data.keys():
		if string[0] == "w":
			var id: int = int(string.erase(0, 1))
			if id > $SlotLabel/SlotButton.get_item_count() - 1:
				$SlotLabel/SlotButton.add_item(system_data[string])
			else:
				$SlotLabel/SlotButton.set_item_text(id, system_data[string])
		else:
			final_id += 1
	refresh_data(weapon_selected)

func refresh_data(id: int) -> void:
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Weapon")
	var weapon_data: Dictionary = json_dictionary["weapon" + String(id)]
	json_dictionary = get_parent().get_parent().call("read_data", "System")
	var system_data: Dictionary = json_dictionary["stats"]
	$NameLabel/NameText.text = json_dictionary["name"]
	var icon: String = weapon_data["icon"]
	if icon != "":
		icon_path = icon
		$IconLabel/IconSprite.texture = load(icon)
	$DescLabel/DescText.text = weapon_data["description"]
	$WTypeLabel/WTypeButton.selected = int(weapon_data["weapon_type"])
	$SlotLabel/SlotButton.selected = int(weapon_data["slot_type"])
	$PriceLabel/PriceSpin.value = int(weapon_data["price"])
	$ElementLabel/ElementButton.selected = int(weapon_data["element"])
	$StatsLabel/StatsContainer/DataContainer/StatNameCont/StatNameList.clear()
	$StatsLabel/StatsContainer/DataContainer/StatValueCont/StatValueList.clear()
	for i in range(system_data.size()):
		var stat_name: String = system_data[String(i)]
		var weapon_stat_formula: Dictionary = weapon_data["stat_list"]
		var stat_formula: String = ""
		if weapon_stat_formula.has(stat_name):
			stat_formula = weapon_stat_formula[stat_name]
		else:
			stat_formula = "0"
		$StatsLabel/StatsContainer/DataContainer/StatNameCont/StatNameList.add_item(stat_name)
		$StatsLabel/StatsContainer/DataContainer/StatValueCont/StatValueList.add_item(stat_formula)
	if weapon_data.has("effects") == true:
		clear_effect_list()
		var effect_list: Array = weapon_data["effects"]
		for effect in effect_list:
			add_effect_list(effect["name"], effect["data_id"], effect["value1"], effect["value2"])

func _on_AddWeaponButton_pressed() -> void:
	$WeaponButton.add_item("NewWeapon")
	var id: int = $WeaponButton.get_item_count() - 1
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Weapon")
	var weapon_data: Dictionary
	var weapon_stats: Dictionary
	weapon_data["name"] = "NewWeapon"
	weapon_data["icon"] = ""
	weapon_data["description"] = "New created weapon"
	weapon_data["weapon_type"] = 0
	weapon_data["slot_type"] = 0
	weapon_data["price"] = 50
	weapon_data["element"] = 0
	weapon_stats["hp"] = "0"
	weapon_stats["mp"] = "0"
	weapon_stats["atk"] = "0"
	weapon_stats["def"] = "0"
	weapon_stats["int"] = "0"
	weapon_stats["res"] = "0"
	weapon_stats["spd"] = "0"
	weapon_stats["luk"] = "0"
	weapon_data["stat_list"] = weapon_stats
	json_dictionary["weapon" + String(id)] = weapon_data
	get_parent().get_parent().call("store_data", "Weapon", json_dictionary)

func _on_RemoveWeapon_pressed() -> void:
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Weapon")
	if json_dictionary.keys().size() > 1:
		var weapon_id: int = weapon_selected
		while weapon_id < (json_dictionary.keys().size() - 1):
			json_dictionary["weapon"+String(weapon_id)] = json_dictionary["weapon"+String(weapon_id+1)]
			weapon_id += 1
		json_dictionary.erase("weapon"+String(weapon_id))
		get_parent().get_parent().call("store_data", "Weapon", json_dictionary)
		$WeaponButton.remove_weapon(weapon_selected)
		if weapon_selected == 0:
			$WeaponButton.select(weapon_selected + 1)
			weapon_selected += 1
		else:
			$WeaponButton.select(weapon_selected + 1)
			weapon_selected -= 1
		$WeaponButton.select(weapon_selected)
		refresh_data(weapon_selected)

func _on_WeaponSaveButton_pressed() -> void:
	save_weapon_data()
	refresh_data(weapon_selected)

func _on_Search_pressed() -> void:
	$IconLabel/IconSearch.popup_centered()

func _on_IconSearch_file_selected(path: String) -> void:
	icon_path = path
	$IconLabel/IconSprite.texture = load(path)

func _on_WeaponButton_item_selected(id: int) -> void:
	weapon_selected = id
	refresh_data(id)

func save_weapon_data() -> void:
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Weapon")
	var weapon_data: Dictionary = json_dictionary["weapon"+String(weapon_selected)]
	var weapon_stat_formula: Dictionary = weapon_data["stat_list"]
	var effect_list: Array
	weapon_data["name"] = $NameLabel/NameText.text
	$WeaponButton.set_item_text(weapon_selected, $NameLabel/NameText)
	weapon_data["icon"] = icon_path
	weapon_data["description"] = $DescLabel/DescText.text
	weapon_data["weapon_type"] = $WTypeLabel/WTypeButton.selected
	weapon_data["slot_type"] = $SlotLabel/SlotButton.selected
	weapon_data["price"] = $PriceLabel/PriceSpin.value
	weapon_data["element"] = $ElementLabel/ElementButton.selected
	var items: int = $StatsLabel/StatsContainer/DataContainer/StatNameCont/StatNameList.get_item_count()
	for i in range(items):
		var stat: String = $StatsLabel/StatsContainer/DataContainer/StatNameCont/StatNameList.get_item_text(i)
		var formula: String = $StatsLabel/StatsContainer/DataContainer/StatValueCont/StatValueList.get_item_text(i)
		weapon_stat_formula[stat] = formula
	var effect_size: int = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.get_skill_count()
	for i in effect_size:
		var effect_data: Dictionary
		effect_data["name"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.get_skill_text(i)
		effect_data["data_id"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.get_skill_text(i)
		effect_data["value1"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.get_skill_text(i)
		effect_data["value2"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2.get_skill_text(i)
		effect_list.append(effect_data)
	weapon_data["effects"] = effect_list
	get_parent().get_parent().call("store_data", "Weapon", json_dictionary)

func _on_StatValueList_item_activated(index: int) -> void:
	var stat_name: String = $StatsLabel/StatsContainer/DataContainer/StatNameCont/StatNameList.get_item_text(index)
	var stat_formula: String = $StatsLabel/StatsContainer/DataContainer/StatValueCont/StatValueList.get_item_text(index)
	$StatEditor/StatLabel.text = stat_name
	$StatEditor/StatEdit.text = stat_formula
	stat_edit = index
	$StatEditor.show()

func _on_OkButton_pressed() -> void:
	var stat_formula: String = $StatEditor/StatEdit.text
	$StatsLabel/StatsContainer/DataContainer/StatValueCont/StatValueList.set_item_text(stat_edit, stat_formula)
	save_weapon_data()
	stat_edit = -1
	$StatEditor.hide()

func _on_CancelButton_pressed() -> void:
	stat_edit = -1
	$StatEditor.hide()

func  _on_AddWeaponEffect_pressed() -> void:
	get_parent().get_parent().call("open_effect_manager", "Weapon")

func _on_RemoveWeaponEffect_pressed() -> void:
	var id: int = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.get_selected_items()[0]
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.remove_item(id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.remove_item(id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.remove_item(id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2.remove_item(id)

func add_effect_list(name: String, data_id: int, value1: String, value2: String) -> void:
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.add_item(name)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.add_item(data_id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.add_item(value1)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2.add_item(value2)

func clear_effect_list() -> void:
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.clear()
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.clear()
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.clear()
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2.clear()

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
